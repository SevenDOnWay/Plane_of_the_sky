using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class MenuController : MonoBehaviour {

    [SerializeField] GameObject[] navPanels;
    [SerializeField] GameObject navBar;
    [SerializeField] GameObject playAgainPanel;

    private GameObject playPanel;
    private CanvasGroup playPanelCanvasGroup;

    [SerializeField] GameObject game;

    [SerializeField] Animator animator;

    private string stringButtonPlay = "B_Play";
    private UnityEngine.UI.Button playButton;

    void Awake() {
        playPanel = navPanels[2]; // 2 is play panel index
        var buttonTransform = navBar.transform.Find(stringButtonPlay);
        playButton= buttonTransform.GetComponent<UnityEngine.UI.Button>();
        playButton.Select();


        playPanelCanvasGroup = playPanel.GetComponent<CanvasGroup>();

        PlayAgainPannel.ClickHome += BackToHome;
    }

    public void ShowPanelByIndex(int index) {
        //TODO add transition effect

        foreach ( var panel in navPanels) {
            panel.SetActive(false);
        }

        navPanels[index].SetActive(true);

        AudioManager.Instance.PlayUiButton();

    }

    public void OnClickPlayPannel() {
        Debug.Log(StateManager.Instance.isPlaying);

        navBar.SetActive(false);
        game.SetActive(true);

        AudioManager.Instance.PlayBackgroundMusic("event:/Background_Music/AmbientGamePlay");

        playPanelCanvasGroup.blocksRaycasts = false;

        StateManager.Instance.OnGameEnded += ShowPlayAgainPanel;

        //TODO add effect, sfx

        StateManager.Instance.StartGame();
    }

    void BackToHome() {
        playButton.Select();
    }

    void ShowPlayAgainPanel() {
        playAgainPanel.SetActive(true);
        playPanelCanvasGroup.blocksRaycasts = true;
    }

    #region add transition between panels
    // i tried but all the time it was not working, so i just commented it out

    async Task TransitionStart() {
        animator.SetTrigger("IsTransitionStart");

        await WaitForAnimation("IsTransitionStart");
    }

    async Task TransitionEnd() {
        animator.SetTrigger("IsTransitionEnd");

        await WaitForAnimation("IsTransitionEnd");
    }

    async Task WaitForAnimation(string triggerName) {
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(triggerName)) await Task.Yield();
    }

    #endregion
}
