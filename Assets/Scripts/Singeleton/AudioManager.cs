using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    private EventInstance currentBackgroundMusic;

    private VCA masterVCA;
    private VCA musicVCA;
    private VCA sfxVCA;


    FMOD.Studio.System fmodSystem;

    private void Awake() {
        if ( Instance != null && Instance != this ) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize audio settings or load audio clips if necessary
        InitializeFMOD();

        PlayBackgroundMusic("event:/BackgroundMusic/MainTheme");
    }

    void InitializeFMOD() {
        // Get FMOD system
        fmodSystem = FMODUnity.RuntimeManager.StudioSystem;

        // Load banks (if not using StudioBankLoader component)
        if ( !RuntimeManager.HaveAllBanksLoaded ) {
            Debug.LogWarning("FMOD banks not fully loaded. Ensure banks are loaded before accessing events.");
            return;
        }

        VCA masterVCA = RuntimeManager.GetVCA("vca:/Master");
        VCA musicVCA = RuntimeManager.GetVCA("vca:/Music");
        VCA sfxVCA   = RuntimeManager.GetVCA("vca:/SFX");

        if ( masterVCA.isValid() ) this.masterVCA.setVolume(PlayerDataManager.Instance.playerData.masterVolume);
        if ( musicVCA.isValid() ) this.musicVCA.setVolume(PlayerDataManager.Instance.playerData.musicVolume);
        if ( sfxVCA.isValid() ) this.sfxVCA.setVolume(PlayerDataManager.Instance.playerData.sfxVolume);
    }

    public void PlaySFX( string eventPath ) {
        try {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventPath);
            eventInstance.start();
            eventInstance.release(); // Release to free memory after playing
        }
        catch ( System.Exception e ) {
            Debug.LogWarning($"Event {eventPath} not found: {e.Message}");
        }
    }

    public void PlayBackgroundMusic( string eventPath ) {

        if ( !IsEventPathValid(eventPath) ) return;

        ClearBackgroundMusic();

        EventInstance eventInstance = RuntimeManager.CreateInstance(eventPath);

        try {
            eventInstance.start();
            currentBackgroundMusic = eventInstance;
        }
        catch ( System.Exception e ) {
            Debug.LogWarning($"Event {eventPath} not found: {e.Message}");
            eventInstance.release();
            return;
        }
    }

    void ClearBackgroundMusic() {
        currentBackgroundMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        currentBackgroundMusic.release();
    }

    public void StopSound( EventInstance eventInstance ) {
        if ( eventInstance.isValid() ) {
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstance.release();
        }
    }

    public bool IsEventPathValid( string path ) {
        // Check if the path is null or empty
        if ( string.IsNullOrEmpty(path) ) {
            return false;
        }

        try {
            // Get the event description for the path
            FMOD.Studio.EventDescription eventDescription;
            FMODUnity.RuntimeManager.StudioSystem.getEvent(path, out eventDescription);

            // Check if the event description is valid
            return eventDescription.isValid();
        }
        catch ( FMODUnity.EventNotFoundException ) {
            // Handle the case where the event is not found
            return false;
        }
        catch ( System.Exception ex ) {
            // Handle any other unexpected errors
            Debug.LogError($"Error checking event path '{path}': {ex.Message}");
            return false;
        }
    }

    public void ChangeMasterVolume( float volume ) {
        masterVCA.setVolume(volume);

        currentBackgroundMusic.setVolume(volume); // Apply to current background music if playing
    }

    public void ChangeMusicVolume( float volume ) {
        musicVCA.setVolume(volume);

        currentBackgroundMusic.setVolume(volume); // Apply to current background music if playing
    }

    public void ChangeSfxVolume( float volume ) {
        sfxVCA.setVolume(volume);
    }

    public void PlayUiButton() => PlaySFX("event:/UIButton");



}
