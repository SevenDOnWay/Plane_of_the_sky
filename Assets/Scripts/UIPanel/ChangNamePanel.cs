using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangNamePanel : MonoBehaviour {
    [SerializeField] TextMeshProUGUI t_name;

    public void OnClickOk() {
        PlayerDataManager.Instance.playerData.name = t_name.text;

        AudioManager.Instance.PlayUiButton();

        PlayerDataManager.Instance.Save();

        gameObject.SetActive(false);
    }

    public void OnClickCancel() {
        AudioManager.Instance.PlayUiButton();

        gameObject.SetActive(false);
    }

}
