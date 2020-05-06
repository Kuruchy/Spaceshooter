using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement {
    public class Menu : MonoBehaviour {
        public void StartGame() {
            SceneManager.LoadScene(1);
        }
        
        public void OpenSettings() {
        }
        
        public void ExitGame() {
            Application.Quit();
        }
    }
}