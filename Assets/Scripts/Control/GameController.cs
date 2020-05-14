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

        private int score;

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
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
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
                    Instantiate(hazard, hazardPosition, hazardRotation);
                    yield return new WaitForSeconds(spawnWait);
                }

                yield return new WaitForSeconds(waveWait);

                if (!gameOver) continue;
                restartText.text = "Press (R) for restart!";
                restart = true;
                break;
            }
        }

        public void AddScore(int newScoreValue) {
            score += newScoreValue;
            UpdateScore();
        }

        private void UpdateScore() {
            scoreText.text = "Score: " + score;
        }

        public void GameOver() {
            gameOverText.text = "GAME OVER";
            gameOver = true;
        }
    }
}