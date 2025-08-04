using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditscene : MonoBehaviour {

    private int waittime = 15;

    private void Awake() {
        StartCoroutine(WaitForCredit());
    }

    IEnumerator WaitForCredit() {
        yield return new WaitForSeconds(waittime);

        SceneManager.LoadScene(0);
    }

}
