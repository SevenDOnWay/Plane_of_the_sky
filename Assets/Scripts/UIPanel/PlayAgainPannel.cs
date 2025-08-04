using System;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayAgainPannel : MonoBehaviour {

    [SerializeField] GameObject playAgainPannel;
    [SerializeField] GameObject navBar;
    [SerializeField] GameObject game;

    [SerializeField] TextMeshProUGUI t_TravalDistance;
    [SerializeField] TextMeshProUGUI t_Score;

    public static event Action ClickHome;


    void OnEnable() {
        StateManager.Instance.OnGameEnded += ShowPlayAgainPanel;
    }

    void OnDisable() {
        StateManager.Instance.OnGameEnded -= ShowPlayAgainPanel;
    }

    void ShowPlayAgainPanel() {
        playAgainPannel.SetActive(true);

        t_TravalDistance.text = "Travel Distance: ";

    }

    public void OnClickHome() {
        playAgainPannel.SetActive(false);
        ModifyObjectState(game);
        navBar.SetActive(true);
        
        AudioManager.Instance.PlayBackgroundMusic("event:/Background_Music/MainMenu");

        ClickHome?.Invoke();
    }

    private void ModifyObjectState(GameObject obj) {
        obj.SetActive(false);

        foreach (Transform child in obj.transform) {
            child.gameObject.SetActive(true);
        }

    }

    public void OnClickScreenShot() {
        string folderPath = Path.Combine(Application.persistentDataPath, "Screenshots");
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, $"screenshot_{System.DateTime.Now:MMMM dd, yyyy}.png");

        ScreenCapture.CaptureScreenshot(filePath);

        Debug.Log(filePath + " saved successfully.");
    }

    public void OnClickShare() {
        ShareOnAndroid();
    }

    private void ShareOnAndroid() {
#if UNITY_EDITOR
        Debug.LogWarning("Sharing is working not working on editor");
#else
#if UNITY_ANDROID

                    var shareSubject = "Play POP IT on your phone"; //Subject text
                    var shareMessage = "Get POP IT game from this link: " + //Message text
                                       "https://play.google.com/store/apps/"; //Your link


                    AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
                    AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

                    intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
                    //put text and subject extra
                    intentObject.Call<AndroidJavaObject>("setType", "text/plain");

                    intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);
                    intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);

                    //call createChooser method of activity class
                    AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                    AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your high score");
                    currentActivity.Call("startActivity", chooser);
#elif UNITY_IOS
                    Debug.Log("Sharing is not implemented for iOS yet.");
#endif
#endif
    }
}
