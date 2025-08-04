using UnityEngine;

public class ScaleToFitScreen : MonoBehaviour {

    public bool scaleForAllChildren;

    public bool scaleWidth;
    public bool scaleHeight;

    [Range(0,100)] public int percentageWidth = 100;
    [Range(0,100)] public int percentageHeight = 100;



    private SpriteRenderer sr;


    private void Start() {
        sr = GetComponent<SpriteRenderer>();

        // world height is always camera's orthographicSize * 2
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // to scale the game object we divide the world screen width with the
        // size x of the sprite, and we divide the world screen height with the
        // size y of the sprite

        if ( scaleWidth ) {
            worldScreenWidth *= (percentageWidth / 100f);
        }
        if ( scaleHeight ) {
            worldScreenHeight *= (percentageHeight / 100f);
        }

        transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x,
            worldScreenHeight / sr.sprite.bounds.size.y,
            1);

        if ( scaleForAllChildren ) {
            SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach ( SpriteRenderer child in childRenderers ) {
                // Skip the parent object's SpriteRenderer to avoid double-scaling
                if ( child != sr ) {
                    child.transform.localScale = new Vector3(
                        worldScreenWidth / child.sprite.bounds.size.x,
                        worldScreenHeight / child.sprite.bounds.size.y,
                        1);
                }
            }
        }
    }

}