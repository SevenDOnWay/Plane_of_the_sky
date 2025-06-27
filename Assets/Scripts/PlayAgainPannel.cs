using UnityEngine;

public class PlayAgainPannel : MonoBehaviour {

    [SerializeField] GameObject playAgainPannel;


    void OnEnable() {
        StateController.Instance.OnGameEnded += ShowPlayAgainPanel;
    }

    void OnDisable() {
        StateController.Instance.OnGameEnded -= ShowPlayAgainPanel;
    }

    void ShowPlayAgainPanel() {
        playAgainPannel.SetActive(true);
    }

}
