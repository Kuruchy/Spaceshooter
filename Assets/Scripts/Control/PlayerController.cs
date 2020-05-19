using System.Collections;
using Core;
using Mirror;
using UnityEngine;

namespace Control {
    public class PlayerController : NetworkBehaviour {
        public AudioSource shotClip;

        [SerializeField] private float speed;
        [SerializeField] private float tilt;
        [SerializeField] private float deadZone = 0.25f;

        [SerializeField] private GameObject shot;
        [SerializeField] private Transform shotSpawn;
        [SerializeField] private Transform shotLSpawn;
        [SerializeField] private Transform shotRSpawn;
        [SerializeField] private Transform shield;

        [SerializeField] private float paddingX = 1f;
        [SerializeField] private float paddingZ = 2f;

        [SerializeField] private float fireRate = 0.4f;
        private BoxCollider boundaryCollider;

        private bool hasExtraShots;
        private float extraShotsTimeOut = 5f;
        private float baseFireRate = 0.4f;
        private float timeSinceLastShot;

        private Boundary boundary;
        private Rigidbody playerRb;
        private Camera gameCamera;

        private Coroutine fireCoroutine;

        private OfflineController offlineController;

        private Joystick joystick;

        private void Awake() {
            playerRb = GetComponent<Rigidbody>();
            gameCamera = Camera.main;
            boundaryCollider = GameObject.FindGameObjectWithTag("Boundary").GetComponent<BoxCollider>();
            joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
            offlineController = FindObjectOfType<OfflineController>();
        }

        private void Start() {
            SetupBoundaries();
        }

        private void SetupBoundaries() {
            boundary = new Boundary {
                xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + paddingX,
                xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - paddingX,
                zMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).z + paddingZ,
                zMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).z - paddingZ
            };

            boundaryCollider.size = new Vector3(
                Mathf.Abs(boundary.xMax - boundary.xMin) + 2,
                1,
                Mathf.Abs(boundary.zMax - boundary.zMin) + 4
            );
        }

        private void FixedUpdate() {
            if (!isLocalPlayer && !offlineController.isOffline) return;

            playerRb.velocity = GetMovement() * speed;

            playerRb.position = new Vector3(
                Mathf.Clamp(playerRb.position.x, boundary.xMin, boundary.xMax),
                0,
                Mathf.Clamp(playerRb.position.z, boundary.zMin, boundary.zMax)
            );

            playerRb.rotation = Quaternion.Euler(0, 0, playerRb.velocity.x * -tilt);
        }

        private Vector3 GetMovement() {
#if UNITY_ANDROID
            var movement = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
#else
            var movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#endif
            if (movement.magnitude < deadZone) {
                movement = Vector3.zero;
            } else {
                movement = movement.normalized * ((movement.magnitude - deadZone) / (1 - deadZone));
            }

            return movement;
        }

        private void Update() {
            if (!isLocalPlayer && !offlineController.isOffline) return;
#if UNITY_ANDROID
            if (Input.GetButtonDown("Fire1")) StartFire();
            if (Input.GetButtonUp("Fire1")) StopFire();
#else
            if (Input.GetButtonDown("Fire1")) StartFire();
            if (Input.GetButtonUp("Fire1")) StopFire();
#endif
        }

        private void StopFire() {
            if (fireCoroutine != null) StopCoroutine(fireCoroutine);
        }

        private void StartFire() {
            fireCoroutine = !(Time.time - timeSinceLastShot > fireRate) ? null : StartCoroutine(Fire());
        }

        private IEnumerator Fire() {
            while (true) {
                timeSinceLastShot = Time.time;
                shotClip.Play();
                if (!hasExtraShots) {
                    SpawnSingleShot();
                } else {
                    SpawnDualShot();
                }

                yield return new WaitForSeconds(fireRate);
            }
        }

        private void SpawnSingleShot() {
            var instance = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            if (!offlineController.isOffline) CmdSpawnSingleShot(instance);
        }

        private void SpawnDualShot() {
            var instance1 = Instantiate(shot, shotLSpawn.position, shotLSpawn.rotation);
            var instance2 = Instantiate(shot, shotRSpawn.position, shotRSpawn.rotation);
            if (!offlineController.isOffline) CmdSpawnDualShot(instance1, instance2);
        }

        [Command]
        private void CmdSpawnSingleShot(GameObject instance) => NetworkServer.Spawn(instance);

        [Command]
        private void CmdSpawnDualShot(GameObject instance1, GameObject instance2) {
            NetworkServer.Spawn(instance1);
            NetworkServer.Spawn(instance2);
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

        private void CreateShield() {
            var instance = Instantiate(shield, gameObject.transform.position, gameObject.transform.rotation);
            instance.transform.parent = gameObject.transform;
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