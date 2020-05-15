using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control {
    public class GameController : NetworkBehaviour {
        public GameObject[] hazards;
        public Vector3 spawnValues;
        public int hazardCount;
        public float spawnWait;
        public float startWait;
        public float waveWait;

        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI restartText;
        public TextMeshProUGUI gameOverText;
        private bool restart;
        private bool gameOver;
        
        private OfflineController offlineController;

        private int score;

        private void Awake() {
            offlineController = FindObjectOfType<OfflineController>();
        }

        private void Start() {
            gameOver = false;
            restart = false;
            gameOverText.text = "";
            restartText.text = "";
            score = 0;
            UpdateScore();
            StartCoroutine(SpawnWaves());
        }

        private void Update() {
            if (!restart) return;
            CheckRestart();
        }

        public void AddScore(int newScoreValue) {
            score += newScoreValue;
            UpdateScore();
        }

        public void GameOver() {
            gameOverText.text = "GAME OVER";
            gameOver = true;
        }

        private IEnumerator SpawnWaves() {
            yield return new WaitForSeconds(startWait);
            while (true) {
                for (var i = 1; i <= hazardCount; i++) {
                    var hazard = hazards[Random.Range(0, hazards.Length)];
                    var hazardPosition = new Vector3(
                        Random.Range(-spawnValues.x, spawnValues.x),
                        spawnValues.y,
                        spawnValues.z
                    );
                    var hazardRotation = Quaternion.identity;
                    SpawnHazard(hazard, hazardPosition, hazardRotation);
                    yield return new WaitForSeconds(spawnWait);
                }

                yield return new WaitForSeconds(waveWait);

                if (!gameOver) continue;
                restartText.text = GetRestartText();
                restart = true;
                break;
            }
        }
        
        private void SpawnHazard(GameObject hazard, Vector3 hazardPosition, Quaternion hazardRotation) {
            var instance = Instantiate(hazard, hazardPosition, hazardRotation);
            if (!offlineController.isOffline) NetworkServer.Spawn(instance);
        }

        private static string GetRestartText() {
#if UNITY_ANDROID
            return "Tap to restart!";
#else
            return "Press (R) for restart!";
#endif
        }

        private static void CheckRestart() {
#if UNITY_ANDROID
            if (Input.GetTouch(0).phase == TouchPhase.Began) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
#else
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
#endif
        }

        private void UpdateScore() {
            scoreText.text = "Score: " + score;
        }
    }
}