using System.Collections;
using Core;
using UnityEngine;

namespace Control {
    public class PlayerController : MonoBehaviour {
        public float speed;
        public float tilt;
        public AudioSource shotClip;

        [SerializeField] private GameObject shot;
        [SerializeField] private Transform shotSpawn;
        [SerializeField] private Transform shotLSpawn;
        [SerializeField] private Transform shotRSpawn;
        [SerializeField] private Transform shield;

        [SerializeField] private float paddingX = 1f;
        [SerializeField] private float paddingZ = 2f;

        [SerializeField] private float fireRate = 0.4f;
        [SerializeField] private BoxCollider boundaryCollider;

        private bool hasExtraShots;
        private float extraShotsTimeOut = 5f;
        private float baseFireRate = 0.4f;
        private float timeSinceLastShot;

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
                zMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).z - paddingZ
            };

            boundaryCollider.size =
                new Vector3(Mathf.Abs(boundary.xMax - boundary.xMin), 1, Mathf.Abs(boundary.zMax - boundary.zMin));
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
                fireCoroutine = !(Time.time - timeSinceLastShot > fireRate) ? null : StartCoroutine(Fire());
            }

            if (Input.GetButtonUp("Fire1")) {
                if (fireCoroutine != null) StopCoroutine(fireCoroutine);
            }
        }

        private IEnumerator Fire() {
            while (true) {
                timeSinceLastShot = Time.time;
                shotClip.Play();
                if (!hasExtraShots) {
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                } else {
                    Instantiate(shot, shotLSpawn.position, shotLSpawn.rotation);
                    Instantiate(shot, shotRSpawn.position, shotRSpawn.rotation);
                }

                yield return new WaitForSeconds(fireRate);
            }
        }

        private IEnumerator AddExtraShots() {
            hasExtraShots = true;
            yield return new WaitForSeconds(extraShotsTimeOut);
            hasExtraShots = false;
        }

        private IEnumerator IncreaseFireRate() {
            fireRate = baseFireRate / 2;
            yield return new WaitForSeconds(extraShotsTimeOut);
            fireRate = baseFireRate;
        }

        public void CreateShield() {
            Instantiate(
                    shield,
                    gameObject.transform.position,
                    gameObject.transform.rotation
                )
                .transform.parent = gameObject.transform;
        }

        public void TakeRedPill() {
            if (!hasExtraShots) StartCoroutine(AddExtraShots());
        }

        public void TakeBluePill() {
            if (GameObject.FindWithTag("Shield") == null) CreateShield();
        }

        public void TakeYellowPill() {
            if (fireRate >= baseFireRate) StartCoroutine(IncreaseFireRate());
        }
    }
}