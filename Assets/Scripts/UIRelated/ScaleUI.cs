using UnityEngine;
using UnityEngine.UI;

public class ScaleUI : MonoBehaviour {
    public bool scaleForAllChildren = true; // Scale all child UI elements (sliders, buttons, etc.)
    public bool scaleWidth = true;         // Scale width to match percentage
    public bool scaleHeight = true;        // Scale height to match percentage
    public bool preserveAspectRatio;       // Preserve aspect ratio when scaling
    public bool updateOnScreenResize;      // Update scaling on screen resize

    [Range(0, 100)] public int percentageWidth = 80;  // Default to 80% of screen width
    [Range(0, 100)] public int percentageHeight = 80; // Default to 80% of screen height

    private RectTransform rectTransform;
    private Vector2 lastScreenSize;

    private void Start() {
        InitializeAndScale();
        lastScreenSize = new Vector2(Screen.width, Screen.height);
    }

    private void Update() {
        if ( updateOnScreenResize ) {
            Vector2 currentScreenSize = new Vector2(Screen.width, Screen.height);
            if ( currentScreenSize != lastScreenSize ) {
                InitializeAndScale();
                lastScreenSize = currentScreenSize;
            }
        }
    }

    private void InitializeAndScale() {
        // Get the RectTransform of the options panel
        rectTransform = GetComponent<RectTransform>();
        if ( rectTransform == null ) {
            Debug.LogWarning($"No RectTransform found on {gameObject.name}. This script requires a UI element.");
            return;
        }

        // Find the parent Canvas
        Canvas canvas = GetComponentInParent<Canvas>();
        if ( canvas == null ) {
            Debug.LogWarning($"No Canvas found in parent hierarchy of {gameObject.name}. Using screen size for scaling.");
        }

        // Get reference size (CanvasScaler reference resolution or screen size)
        Vector2 referenceSize = new Vector2(Screen.width, Screen.height);
        CanvasScaler canvasScaler = canvas ? canvas.GetComponent<CanvasScaler>() : null;
        if ( canvasScaler != null && canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize ) {
            referenceSize = canvasScaler.referenceResolution;
        }

        // Calculate target size based on percentages
        float targetWidth = referenceSize.x * (percentageWidth / 100f);
        float targetHeight = referenceSize.y * (percentageHeight / 100f);

        // Scale the main options panel
        ScaleElement(rectTransform, targetWidth, targetHeight);

        // Scale all child UI elements if enabled
        if ( scaleForAllChildren ) {
            RectTransform[] childTransforms = GetComponentsInChildren<RectTransform>();
            foreach ( RectTransform childRt in childTransforms ) {
                if ( childRt != rectTransform ) // Skip the parent RectTransform
                {
                    ScaleElement(childRt, targetWidth, targetHeight);
                }
            }
        }
    }

    private void ScaleElement( RectTransform rt, float targetWidth, float targetHeight ) {
        Vector2 originalSize = rt.sizeDelta;
        Vector2 newScale = rt.localScale;

        // Avoid division by zero
        if ( originalSize.x == 0 || originalSize.y == 0 ) {
            Debug.LogWarning($"RectTransform on {rt.gameObject.name} has zero size (sizeDelta: {originalSize}). Skipping scaling.");
            return;
        }

        // Calculate scale factors
        float widthScale = scaleWidth ? targetWidth / originalSize.x : rt.localScale.x;
        float heightScale = scaleHeight ? targetHeight / originalSize.y : rt.localScale.y;

        // Preserve aspect ratio if enabled
        if ( preserveAspectRatio ) {
            float minScale = Mathf.Min(widthScale, heightScale);
            newScale.x = minScale;
            newScale.y = minScale;
        }
        else {
            newScale.x = widthScale;
            newScale.y = heightScale;
        }

        rt.localScale = new Vector3(newScale.x, newScale.y, 1);
    }
}
