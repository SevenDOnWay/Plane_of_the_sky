using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour {
    [Header("Difficulty")]
   

    private TextMeshProUGUI t_TravalDistance;

    private void Start() {
        t_TravalDistance = GetComponent<TextMeshProUGUI>();
    }


    private void Update() {
        if ( StateManager.Instance.isPlaying ) {
            t_TravalDistance.text = ((int)ScoreManager.Instance.travelDistance).ToString();
        }
    }


}
