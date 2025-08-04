using UnityEngine;

public class EmailPanel : MonoBehaviour {

    public void OnClickClose() {
        AudioManager.Instance.PlayUiButton();

        gameObject.SetActive(false);
    }

}
