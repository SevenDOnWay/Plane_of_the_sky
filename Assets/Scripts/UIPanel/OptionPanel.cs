using System.Runtime.InteropServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionPanel : MonoBehaviour {

    [SerializeField] GameObject emailPanel;

    float masterVolume;
    float musicVolume;
    float sfxVolume;

    public void OnClickOutSidePanel() {
        gameObject.SetActive(false);

        SaveVolumeProperties();
        StateManager.Instance.ResumeGame();
    }

    public void OnClickClose() {
        AudioManager.Instance.PlayUiButton();

        gameObject.SetActive(false);

        SaveVolumeProperties();
        StateManager.Instance.ResumeGame();
    }

    void SaveVolumeProperties() {
        PlayerDataManager.Instance.playerData.masterVolume = masterVolume;
        PlayerDataManager.Instance.playerData.musicVolume = musicVolume;
        PlayerDataManager.Instance.playerData.sfxVolume = sfxVolume;

        PlayerDataManager.Instance.Save();
    }

    #region Volume Settings

    public void ChangeMasterVolume(float volume) {
        masterVolume = volume;
        AudioManager.Instance.ChangeMasterVolume(masterVolume);
    }

    public void ChangeMusicVolume(float volume) {
        musicVolume = volume;
        AudioManager.Instance.ChangeMusicVolume(musicVolume);
    }

    public void ChangeSfxVolume(float volume) {
        sfxVolume = volume;
        AudioManager.Instance.ChangeSfxVolume(sfxVolume);
    }

    public void OnClickEmail() { 
        AudioManager.Instance.PlayUiButton();

        emailPanel.SetActive(true);
    }

    #endregion

    public void OnClickCredit() {
        //TODO add transition effect, sfx
        AudioManager.Instance.PlayUiButton();

        // Open the credit panel or perform the desired action
        // For example, you might want to load a new scene or display a credit UI
         SceneManager.LoadScene(1);
    }
}
