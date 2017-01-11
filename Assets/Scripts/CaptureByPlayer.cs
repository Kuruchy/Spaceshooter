using UnityEngine;
using System.Collections;

public class CaptureByPlayer : MonoBehaviour {
    public GameObject upgradeFromVitamin;
    public GameObject upgradeFromVitaminExtra;


    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (gameObject.tag == "Vitamin") {
                if (gameObject.name.Contains("Red")) {
                    // Adds 2 more shots
                    if (!other.GetComponent<PlayerController>().hasExtraShots) {
                        other.GetComponent<PlayerController>().hasExtraShots = true;
                        (Instantiate(
                            upgradeFromVitamin,
                            other.transform.position + upgradeFromVitamin.transform.position,
                            other.transform.rotation
                        ) as GameObject).transform.parent = other.transform;
                        (Instantiate(
                            upgradeFromVitaminExtra,
                            other.transform.position - upgradeFromVitamin.transform.position,
                            other.transform.rotation
                        ) as GameObject).transform.parent = other.transform;
                    }
                } else if (gameObject.name.Contains("Blue")) {
                    // Adds the shield if you dont have one
                    if (!other.GetComponent<PlayerController>().hasShield) {
                        other.GetComponent<PlayerController>().hasShield = true;
                        (Instantiate(
                            upgradeFromVitamin,
                            other.transform.position,
                            other.transform.rotation
                        ) as GameObject).transform.parent = other.transform;
                    } else {
                        other.GetComponent<PlayerController>().hasShield = false;
                    }
                } else if (gameObject.name.Contains("Yellow")) { }

                Destroy(gameObject);
//					Debug.Log("Get Vitamin");
            }
        }
    }
}