using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ParallaxBackGround : MonoBehaviour {
    [System.Serializable]
    public class Mountain {
        [SerializeField] GameObject[] mountains ;


        public GameObject[] Mountains => mountains;

    }

    [System.Serializable]
    public class ParallaxLayer {
        [SerializeField] List<Mountain> layers = new List<Mountain>();
        [SerializeField] float heightOffset = 0f;
        [SerializeField] float baseHeight = 0f;
        [SerializeField, Range(0, 1)] float scrollSpeed = 0.5f;

        public List<Mountain> Layers => layers;
        public float ScrollSpeed => scrollSpeed;
        public float HeightOffset => heightOffset;
        public float BaseHeight => baseHeight;
    }


    [SerializeField] float boundary = -13f;
    [SerializeField] float spawnPositionX = 13f;
    [SerializeField] ParallaxLayer[] layers;

    Dictionary<GameObject, SpriteRenderer> spriteRendererCache;

    void Awake() {
        InitializeCache();
    }

    void InitializeCache() {
        spriteRendererCache = new Dictionary<GameObject, SpriteRenderer>();

        foreach ( var layer in layers ) {
            if ( layer == null ) continue;

            foreach ( var mountain in layer.Layers ) {
                foreach ( var obj in mountain.Mountains ) {
                    if ( obj != null ) {
                        var sprite = obj.GetComponent<SpriteRenderer>();
                        if ( sprite != null ) {
                            spriteRendererCache[obj] = sprite;
                        }
                    }
                }
            }
        }
    }

    void Update() {
        if ( !StateController.Instance.isPlaying ) return;

        foreach ( var layer in layers ) {
            if ( layer == null ) continue;

            foreach ( var mountain in layer.Layers ) {
                foreach ( var obj in mountain.Mountains ) {
                    if ( obj == null ) continue;

                    UpdateMountainPosition(obj, layer, mountain);
                }
            }
        }
    }

    void UpdateMountainPosition(GameObject obj, ParallaxLayer layer, Mountain mountain) {
        Vector3 position = obj.transform.position;

        obj.transform.position += Vector3.left * layer.ScrollSpeed * Time.deltaTime;

        if (spriteRendererCache.TryGetValue(obj, out SpriteRenderer sprite) ) {
            if ( position.x + sprite.bounds.size.x < boundary ) {
                ResetMountainPosition(obj, layer, sprite.bounds.size.x);
            }
        }
    }

    void ResetMountainPosition(GameObject obj, ParallaxLayer layer, float width) {
        float randomHeight = layer.BaseHeight + Random.Range(-layer.HeightOffset, layer.HeightOffset);
        obj.transform.position = new Vector3(spawnPositionX + width, randomHeight, obj.transform.position.z);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(boundary, -10, 0), new Vector3(boundary, 10, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(spawnPositionX, -10, 0), new Vector3(spawnPositionX, 10, 0));
    }

}
