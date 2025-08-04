using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditorInternal.ReorderableList;

public class ParallaxBackGround : MonoBehaviour {

    [System.Serializable]
    public class ParallaxLayer {
        [SerializeField] public GameObject mountainPrefab;
        [HideInInspector] public List<GameObject> mountainInstances = new List<GameObject>();
        [HideInInspector] public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        [SerializeField] public bool mainMountain;
        [SerializeField] public float heightOffset = 0f;
        [SerializeField] public float baseHeight = 0f;
        [SerializeField] public float xSpawnRange = 0f;
        [SerializeField, Range(1,3)] public int amountMountain = 1;
        [SerializeField, Range(0, 10)] public float scrollSpeed = 0f;
    }


    [SerializeField] ParallaxLayer[] mountains;
    [SerializeField] GameObject mountainPool;

    float boundary;

    void Awake() {
        InitializeMountain();
        CalculateWorldSize();
    }

    void InitializeMountain() {
        foreach ( var obj in mountains ) {
            if ( obj.mountainPrefab == null ) continue;
            for ( int i = 0; i < obj.amountMountain; i++ ) {

                Vector3 pos;

                if ( obj.mainMountain ) {
                    pos = new Vector3(0, 0, 0f); // fixed position for main mountain
                }
                else {
                    float posx = Random.Range(-obj.xSpawnRange, obj.xSpawnRange);
                    float posy = obj.baseHeight + Random.Range(-obj.heightOffset, obj.heightOffset);
                    pos = new Vector3(posx, posy, 0f);
                }

                GameObject instance = Instantiate(obj.mountainPrefab, pos, Quaternion.identity, mountainPool.transform);
                obj.mountainInstances.Add(instance);
                obj.spriteRenderers.Add(instance.GetComponent<SpriteRenderer>());
            }
        }
    }

    void CalculateWorldSize() { 
        boundary = WorldSizeManager.Instance.worldScreenWidth;
    }

    void Update() {
        if ( !StateManager.Instance.isPlaying || StateManager.Instance.isPausing) return;

        foreach ( var obj in mountains ) {
            UpdateMountainPosition(obj);
        }
    }

    void UpdateMountainPosition( ParallaxLayer obj ) {
        for ( int i = 0; i < obj.mountainInstances.Count; i++ ) {
            if ( obj.mountainInstances == null ) continue;

            obj.mountainInstances[i].transform.Translate(Vector3.left * obj.scrollSpeed * Time.deltaTime);

            // Check if mountain needs resetting
            if ( obj.mountainInstances[i].transform.position.x + obj.spriteRenderers[i].bounds.extents.x < boundary ) {
                ResetMountainPosition(obj.mountainInstances[i], obj.xSpawnRange, obj.baseHeight, obj.heightOffset);
            }
        }
    }

    IEnumerator ResetMountainPosition( GameObject mountain, float xSpawnRange, float baseHeight, float heightOffset ) {
        var time = Random.Range(1,5);
        yield return new WaitForSeconds(time);


        float randomX = Random.Range(0, xSpawnRange);
        float randomHeight = baseHeight + Random.Range(-heightOffset, heightOffset);
        mountain.transform.localPosition = new Vector3(boundary + randomX, randomHeight, 1);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(boundary, -10, 0), new Vector3(boundary, 10, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(boundary, -10, 0), new Vector3(boundary, 10, 0));
    }

}
