using UnityEngine;

public class MenuController : MonoBehaviour {
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject playScreen;
    [SerializeField] GameObject profileScreen;
    [SerializeField] GameObject skinScreen;
    [SerializeField] GameObject dailyChallengeScreen;
    [SerializeField] GameObject storeScreen;
    [SerializeField] GameObject playAgainPannel;

    [SerializeField] GameObject game;


    void OnEnable() {
        StateController.Instance.OnGameEnded += ShowPlayAgainPanel;
    }

    void Start() {
        menuPanel.SetActive(true);
        playScreen.SetActive(true);
        profileScreen.SetActive(false);
        skinScreen.SetActive(false);
        dailyChallengeScreen.SetActive(false);
        storeScreen.SetActive(false);
        game.SetActive(false);
        playAgainPannel.SetActive(false);
    }

    public void OnClickPlay() {
        menuPanel.SetActive(true);
        playScreen.SetActive(true);
        profileScreen.SetActive(false);
        skinScreen.SetActive(false);
        dailyChallengeScreen.SetActive(false);
        storeScreen.SetActive(false);
        game.SetActive(false);
    }

    public void OnClickProfile() {
        menuPanel.SetActive(true);
        playScreen.SetActive(false);
        profileScreen.SetActive(true);
        skinScreen.SetActive(false);
        dailyChallengeScreen.SetActive(false);
        storeScreen.SetActive(false);
        game.SetActive(false);
    }

    public void OnClickSkin() {
        menuPanel.SetActive(true);
        playScreen.SetActive(false);
        profileScreen.SetActive(false);
        skinScreen.SetActive(true);
        dailyChallengeScreen.SetActive(false);
        storeScreen.SetActive(false);
        game.SetActive(false);
    }

    public void OnClickDailyChallenge() {
        menuPanel.SetActive(true);
        playScreen.SetActive(false);
        profileScreen.SetActive(false);
        skinScreen.SetActive(false);
        dailyChallengeScreen.SetActive(true);
        storeScreen.SetActive(false);
        game.SetActive(false);
    }
        
    public void OnClickStore() {
        menuPanel.SetActive(true);
        playScreen.SetActive(false);
        profileScreen.SetActive(false);
        skinScreen.SetActive(false);
        dailyChallengeScreen.SetActive(false);
        storeScreen.SetActive(true);
        game.SetActive(false);
    }

    public void OnClickPlayPannel() {
        Debug.Log(StateController.Instance.isPlaying);

        menuPanel.SetActive(false);
        playScreen.SetActive(true);
        profileScreen.SetActive(false);
        skinScreen.SetActive(false);
        dailyChallengeScreen.SetActive(false);
        storeScreen.SetActive(false);

        game.SetActive(true);
        StateController.Instance.StartGame();

    }

    void ShowPlayAgainPanel() {
        playAgainPannel.SetActive(true);
    }

    public void ONClickHomeUI() {
        menuPanel.SetActive(true);
        playScreen.SetActive(true);
        profileScreen.SetActive(false);
        skinScreen.SetActive(false);
        dailyChallengeScreen.SetActive(false);
        storeScreen.SetActive(false);
        game.SetActive(false);
        playAgainPannel.SetActive(false);
    }

    public void OnClickPlayAgain() {
        playAgainPannel.SetActive(false);

        //TODO: reset game 



        StateController.Instance.StartGame();
    }

    public void ONClickVideo() {
        //TODO: Implement video ad logic and revice 


        playAgainPannel.SetActive(false);
    }

    public void ONClickSpendMoney() {
        //TODO: Implement spend money logic and revive


        playAgainPannel.SetActive(false);
    }


}
