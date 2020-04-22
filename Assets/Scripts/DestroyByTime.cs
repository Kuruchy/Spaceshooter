using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {
    public float lifetime;

    private void Start() {
        Destroy(gameObject, lifetime);
    }
}