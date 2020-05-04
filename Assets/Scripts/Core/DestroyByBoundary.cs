using UnityEngine;

namespace Core {
    public class DestroyByBoundary : MonoBehaviour {
        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player")) Destroy(other.gameObject);
        }
    }
}