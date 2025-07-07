using UnityEngine;

public class PlayAgainPannel : MonoBehaviour {

    [SerializeField] GameObject playAgainPannel;


    void OnEnable() {
        StateController.OnGameEnded += ShowPlayAgainPanel;
    }

    void OnDisable() {
        StateController.OnGameEnded -= ShowPlayAgainPanel;
    }

    void ShowPlayAgainPanel() {
        playAgainPannel.SetActive(true);
    }

}
