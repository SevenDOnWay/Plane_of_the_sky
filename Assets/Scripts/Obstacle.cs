using UnityEngine;

public class Obstacle : MonoBehaviour {
    [SerializeField] float initSpeed = 5f;
    [SerializeField] float maxSpeed = 12f;
    [SerializeField] float accelerationRate = 0.1f;
    [SerializeField] float destroyDistance = -11f;
    [SerializeField] Vector2 spawnPoint = new Vector2(13, 0);

    [SerializeField] GameObject playAgainPannel;

    private float currentSpeed;

    private void OnEnable() {
        currentSpeed = initSpeed;
    }

    void Update() {
        if ( StateController.Instance.isPlaying ) {
            if ( transform.position.x < destroyDistance ) {
                transform.position = spawnPoint;
                gameObject.SetActive(false);
            }

            if ( currentSpeed < maxSpeed ) {
                currentSpeed += accelerationRate * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
            }

            transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {

            //TODO add sfx, particle effects, play again?, etc.

            Debug.Log("Player hit an obstacle!");
            StateController.Instance.EndGame(); // End the game when the player hits an obstacle
        }
    }

}
