using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement {
    public class Menu : MonoBehaviour {
        public void StartSingleGame() {
            SceneManager.LoadScene(2);
        }
        
        public void GoToLobby() {
            SceneManager.LoadScene(1);
        }
        
        public void GoToMenu() {
            SceneManager.LoadScene(0);
        }
        
        public void StartMultiplayerGame() {
            SceneManager.LoadScene(2);
        }
        
        public void OpenSettings() {
        }
        
        public void ExitGame() {
            Application.Quit();
        }
    }
}