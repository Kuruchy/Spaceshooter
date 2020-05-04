using UnityEngine;

namespace Control {
    public class CameraShake : MonoBehaviour {
        private static readonly int Shake = Animator.StringToHash("Shake");

        public void CamShake() => Camera.main.GetComponent<Animator>().SetTrigger(Shake);
    }
}