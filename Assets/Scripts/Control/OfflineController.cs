using Mirror;
using UnityEngine;

namespace Control {
    public class OfflineController : MonoBehaviour {

        public bool isOffline;

        [SerializeField] private GameObject player;
        [SerializeField] private GameController gameController;
        private bool setup;

        private void Start() {
            if (!isOffline || setup) return;
            CheckEnable();
        }
        
        private void Update() {
            if (!isOffline || setup) return;
            CheckEnable();
        }

        private void CheckEnable() {
            gameController.gameObject.SetActive(true);
            gameController.GetComponent<NetworkIdentity>().enabled = false;
            if (player == null) return;
            player.gameObject.SetActive(true);
            player.GetComponent<NetworkIdentity>().enabled = false;
            player.GetComponent<NetworkTransform>().enabled = false;
            setup = true;
        }
    }
}