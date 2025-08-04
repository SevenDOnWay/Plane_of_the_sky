using System;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    [SerializeField] float initSpeed = 5f;
    [SerializeField] float maxSpeed = 12f;
    [SerializeField] float accelerationRate = 0.1f;
    [SerializeField] float spawnPoint;


    private float currentSpeed;
    private float halfSpriteWidth;



    private void OnEnable() {
        currentSpeed = initSpeed;
    }

    void Start() {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        halfSpriteWidth = spriteRenderer.bounds.size.x;
        spawnPoint = WorldSizeManager.Instance.worldScreenWidth;
    }

    void Update() {
        if ( StateManager.Instance.isPlaying ) {
            if ( transform.position.x + halfSpriteWidth < -(WorldSizeManager.Instance.worldScreenWidth) ) {
                transform.position = new Vector2(spawnPoint - halfSpriteWidth, 0);
                gameObject.SetActive(false);
            }

            if ( currentSpeed < maxSpeed ) {
                currentSpeed += accelerationRate * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
            }

            transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
        }
    }



    void OnTriggerEnter2D( Collider2D collision ) {
        if ( collision.CompareTag("Player") ) {

            //TODO add sfx, particle effects, play again?, etc.

            AudioManager.Instance.PlaySFX("event:/Crashing");

            Debug.Log("Player hit an obstacle!");
            StateManager.Instance.EndGame(); // End the game when the player hits an obstacle
        }
    }

}
