using System;
using UnityEngine;

[Serializable] // to see the boundary class variables in the Player GameObject in a container
public class Boundary2 // is separated from the main class on order to compact the view
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
    public float speed;
    public float tilt;
    public Boundary2 boundary;

    public bool hasShield;
    public bool hasExtraShots;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public AudioSource shotClip;

    private float nextFire;

    private void FixedUpdate() {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        var movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        GetComponent<Rigidbody>().position =
            new Vector3( // we need to limit the position of the sheep inside the window
                Mathf.Clamp(
                    GetComponent<Rigidbody>().position.x,
                    boundary.xMin,
                    boundary.xMax
                ), // Clamp limit the value between two numbers
                0.0f,
                Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

    private void Update() {
        if (Input.GetButton("Fire1") && Time.time > nextFire) {
            shotClip.Play();
            nextFire = Time.time + fireRate;
            var clone = Instantiate(
                shot,
                shotSpawn.position,
                shotSpawn.rotation
            );
        }
    }
}