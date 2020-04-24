using UnityEngine;

public class CaptureByPlayer : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            var playerController = other.GetComponent<PlayerController>();
            
            if (gameObject.CompareTag("Vitamin")) {
                if (gameObject.name.Contains("Red")) {
                    playerController.TakeRedPill();
                } else if (gameObject.name.Contains("Blue")) {
                    playerController.TakeBluePill();
                } else if (gameObject.name.Contains("Yellow")) {
                    playerController.TakeYellowPill();
                }

                Destroy(gameObject);
            }
        }
    }
}