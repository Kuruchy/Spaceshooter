using UnityEngine;

public class WeaponController : MonoBehaviour {
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float delay;

    private void Start() {
        InvokeRepeating(nameof(Fire), delay, fireRate);
    }

    private void Fire() {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        GetComponent<AudioSource>().Play();
    }
}