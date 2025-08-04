using TMPro;
using UnityEditor;
using UnityEngine;

public class ProfilePanel : MonoBehaviour {

    [SerializeField] TextMeshProUGUI t_name;
    [SerializeField] TextMeshProUGUI t_run;
    [SerializeField] TextMeshProUGUI t_bestScore;
    [SerializeField] GameObject changeNamePanel;

    private void OnEnable() {
        UpdateProfile();
    }

    public void OnClickChange() {
        AudioManager.Instance.PlayUiButton();
        changeNamePanel.SetActive(true);

    }

    public void UpdateProfile() {
        t_name.text = PlayerDataManager.Instance.playerData.name;
        t_run.text = $"Run: {PlayerDataManager.Instance.playerData.totaltimePlayed.ToString()}";
        t_bestScore.text = $"HighScore: {PlayerDataManager.Instance.playerData.highScore.ToString()}";
    }
}
