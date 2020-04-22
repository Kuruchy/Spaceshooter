using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;
    public float tilt;

    public bool hasShield;
    public bool hasExtraShots;

    public GameObject shot;
    public Transform shotSpawn;
    public AudioSource shotClip;

    [SerializeField] private float fireRate = 0.4f;
    [SerializeField] private float paddingX = 1f;
    [SerializeField] private float paddingZ = 2f;

    private Boundary boundary;
    private Rigidbody playerRb;
    private Camera gameCamera;

    private Coroutine fireCoroutine;

    private void Start() {
        playerRb = GetComponent<Rigidbody>();
        gameCamera = Camera.main;
        SetupBoundaries();
    }

    private void SetupBoundaries() {
        boundary = new Boundary {
            xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + paddingX,
            xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - paddingX,
            zMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).z + paddingZ,
            zMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + paddingZ
        };
    }

    private void FixedUpdate() {
        var movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        playerRb.velocity = movement * speed;

        playerRb.position = new Vector3(
            Mathf.Clamp(playerRb.position.x, boundary.xMin, boundary.xMax),
            0,
            Mathf.Clamp(playerRb.position.z, boundary.zMin, boundary.zMax)
        );

        playerRb.rotation = Quaternion.Euler(0, 0, playerRb.velocity.x * -tilt);
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1")) {
            fireCoroutine = StartCoroutine(Fire());
        }

        if (Input.GetButtonUp("Fire1")) {
            StopCoroutine(fireCoroutine);
        }
    }

    private IEnumerator Fire() {
        while (true) {
            shotClip.Play();
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            yield return new WaitForSeconds(fireRate);
        }
    }
}