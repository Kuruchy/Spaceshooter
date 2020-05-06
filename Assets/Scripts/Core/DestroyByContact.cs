using System.Collections.Generic;
using Control;
using UnityEngine;

namespace Core {
    public class DestroyByContact : MonoBehaviour {
        public GameObject asteroidExplosion;
        public GameObject playerExplosion;
        public List<GameObject> vitamins;
        public int scoreValue;
        private GameController gameController;
        [SerializeField] private CameraShake shake;

        private void Start() {
            var gameControllerObject = GameObject.FindWithTag("GameController");
            if (gameControllerObject != null) {
                gameController = gameControllerObject.GetComponent<GameController>();
            }

            if (gameControllerObject == null) {
                Debug.Log("Cannot find 'GameController' script");
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Vitamin")) {
                return;
            }

            if (asteroidExplosion != null && !other.CompareTag("Vitamin")) {
                if (shake != null) shake.CamShake();
                Instantiate(asteroidExplosion, transform.position, transform.rotation);
            }

            if (other.CompareTag("Player")) {
                if (gameObject.CompareTag("Vitamin")) {
                    Debug.Log("Get Vitamin");
                } else {
                    Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                    gameController.GameOver();
                }
            }

            gameController.AddScore(scoreValue);
            Destroy(other.gameObject);
            Destroy(gameObject);
            if (vitamins.Count > 0)
                Instantiate(vitamins[Random.Range(0, vitamins.Count)], transform.position, transform.rotation);
        }
    }
}