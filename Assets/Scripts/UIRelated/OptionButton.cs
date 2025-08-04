using UnityEngine;

public class OptionButton : MonoBehaviour {

    [SerializeField] GameObject optionPanel;

    public void onClick() {
        StateManager.Instance.PauseGame();

        optionPanel.SetActive(true);

        //TODO add sfx, transition
        AudioManager.Instance.PlayUiButton();
    }

}
