using Mirror;
using UnityEngine;

namespace Movement {
    public class RandomRotator : NetworkBehaviour {
        public float tumble;

        private void Start() {
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
        }
    }
}