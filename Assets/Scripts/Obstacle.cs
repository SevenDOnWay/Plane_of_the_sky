using UnityEngine;

public class Obstacle : MonoBehaviour {
    [SerializeField] float speed = 5f; 
    [SerializeField] float destroyDistance = -11f; 
    [SerializeField] Vector2 spawnPoint = new Vector2(13, 0);

    void Update() {
        if (StateController.Instance.isPlaying) {
            if (transform.position.x < destroyDistance) {
                transform.position = spawnPoint;
                gameObject.SetActive(false);
            }

            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            
            //TODO add animation, play sfx, score, play again?

            Debug.Log("Obstacle hit the player!");
            StateController.Instance.EndGame(); // End the game when the player hits an obstacle
        }
    }
}
