using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    private int timeBetweenTransition = 10; // in milliseconds

    [SerializeField] GameObject game;

    [SerializeField] Animator animator;

    //public delegate void MyDelegate();
    //public static event MyDelegate myDelegate;

    void Awake() {
        playPanel = navPanels[2];
        playPanelCanvasGroup = playPanel.GetComponent<CanvasGroup>();
    }

    public async void ShowPanelByIndex(int index) {

        await TransitionStart();

        foreach (var panel in navPanels) {
            panel.SetActive(false);
        }
        navPanels[index].SetActive(true);

        await Task.Yield();

        await TransitionEnd();
    }

    public void OnClickPlayPannel() {
        Debug.Log(StateController.Instance.isPlaying);


        navBar.SetActive(false);
        game.SetActive(true);

        playPanelCanvasGroup.blocksRaycasts = false;

        StateController.OnGameEnded += ShowPlayAgainPanel;

        //TODO add effect, sfx


        StateController.Instance.StartGame();

    }

    void ShowPlayAgainPanel() {
        playAgainPanel.SetActive(true);
        playPanelCanvasGroup.blocksRaycasts = true;
    }


    public async Task OnClickPlayAgain() {

        //TODO: reset 

        await OnClickPlayAgain(); //temporary fix to avoid infinite loop

        StateController.Instance.StartGame();
    }





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


}
