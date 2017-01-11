using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {
    public GameObject asteroidExplosion;
    public GameObject playerExplosion;
    public GameObject[] vitamins;
    public int scoreValue;
    private GameController gameController;
    private GameObject vitamin;

    void Start() {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameControllerObject == null) {
            Debug.Log("Cannot find 'GameController' script");
        }

        vitamin = vitamins[Random.Range(0, vitamins.Length)];
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "Vitamin") {
            return;
        }

        if (asteroidExplosion != null && other.tag != "Vitamin") {
            Instantiate(asteroidExplosion, transform.position, transform.rotation);
        }

        if (other.tag == "Player") {
            if (gameObject.tag == "Vitamin") {
                Debug.Log("Get Vitamin");
            } else {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.GameOver();
            }
        }

        gameController.AddScore(scoreValue);
        Destroy(other.gameObject);
        Destroy(gameObject);
        Instantiate(vitamin, transform.position, transform.rotation);
    }
}