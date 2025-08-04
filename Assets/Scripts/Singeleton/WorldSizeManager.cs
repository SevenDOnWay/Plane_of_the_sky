using UnityEngine;

public class WorldSizeManager : MonoBehaviour {

    public static WorldSizeManager Instance { get; private set; }


    [HideInInspector] public float worldScreenHeight;
    [HideInInspector] public float worldScreenWidth;


    private void Awake() {
        if ( Instance != null && Instance != this ) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CalulateWorldSize();
    }

    void CalulateWorldSize() {
        worldScreenHeight = Camera.main.orthographicSize * 2;
        worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
    }


}
