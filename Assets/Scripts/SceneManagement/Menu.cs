using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement {
    public class Menu : MonoBehaviour {
        public void StartSingleGame() {
            SceneManager.LoadScene(1);
        }
        
        public void StartMultiplayerGame() {
            SceneManager.LoadScene(1);
        }
        
        public void OpenSettings() {
        }
        
        public void ExitGame() {
            Application.Quit();
        }
    }
}