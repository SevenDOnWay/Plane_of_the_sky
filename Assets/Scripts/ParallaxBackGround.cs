using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ParallaxBackGround : MonoBehaviour {
    [System.Serializable]
    public class Mountain {
        [SerializeField] GameObject mountains ;


        public GameObject Mountains => mountains;

    }

    [System.Serializable]
    public class ParallaxLayer {
        [SerializeField] List<Mountain> layers = new List<Mountain>();
        [SerializeField] float heightOffset = 0f;
        [SerializeField] float baseHeight = 0f;
        [SerializeField] float xSpawnRange = 0f;
        [SerializeField, Range(0, 10)] float scrollSpeed = 0f;

        public List<Mountain> Layers => layers;
        public float ScrollSpeed => scrollSpeed;
        public float HeightOffset => heightOffset;
        public float BaseHeight => baseHeight;
        public float XSpawnRange => xSpawnRange;
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
            if ( layer?.Layers == null ) continue;

            foreach ( var mountain in layer.Layers ) {
                if ( mountain?.Mountains == null ) continue;

                var sprite = mountain.Mountains.GetComponent<SpriteRenderer>();
                if ( sprite != null ) {
                    spriteRendererCache[mountain.Mountains] = sprite;
                }
            }
        }
    }

    void Update() {
        if ( !StateController.Instance.isPlaying ) return;

        foreach ( var layer in layers ) {
            if ( layer?.Layers == null ) continue;

            foreach ( var mountain in layer.Layers ) {
                if ( mountain?.Mountains == null ) continue;
                UpdateMountainPosition(mountain.Mountains, layer);
            }
        }
    }

    void UpdateMountainPosition(GameObject mountainObj, ParallaxLayer layer) {
        mountainObj.transform.Translate(Vector3.left * layer.ScrollSpeed * Time.deltaTime);

        // Check if mountain needs resetting
        if ( spriteRendererCache.TryGetValue(mountainObj, out SpriteRenderer sprite) ) {
            if ( mountainObj.transform.position.x + sprite.bounds.size.x < boundary ) {
                ResetMountainPosition(mountainObj, layer, sprite.bounds.size.x);
            }
        }
    }

    void ResetMountainPosition(GameObject obj, ParallaxLayer layer, float width) {
        float randomX = Random.Range(0, layer.XSpawnRange);
        float randomHeight = layer.BaseHeight + Random.Range(-layer.HeightOffset, layer.HeightOffset);
        obj.transform.localPosition = new Vector3(spawnPositionX + width + randomX, randomHeight, obj.transform.position.z);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(boundary, -10, 0), new Vector3(boundary, 10, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(spawnPositionX, -10, 0), new Vector3(spawnPositionX, 10, 0));
    }

}
