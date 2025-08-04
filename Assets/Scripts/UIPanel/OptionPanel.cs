using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour {

    [SerializeField] GameObject emailPanel;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    float masterVolume;
    float musicVolume;
    float sfxVolume;

    public void OnEnable() {
        masterVolume = PlayerDataManager.Instance.playerData.masterVolume;
        musicVolume = PlayerDataManager.Instance.playerData.musicVolume;
        sfxVolume = PlayerDataManager.Instance.playerData.sfxVolume;

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }

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
