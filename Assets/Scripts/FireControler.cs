using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FireControler : MonoBehaviour, IPointerDownHandler {
    private Image bgImg;
    public Vector3 inputVector;
    private float nextFire;
    public GameObject shot;
    public Transform[] shotSpawns;
    public float fireRate;

    private void Start() {
        bgImg = GetComponent<Image>();
    }

    public virtual void OnPointerDown(PointerEventData ped) {
        Vector2 pos;

        var arrayOfChildren = GameObject.FindGameObjectWithTag("Player")
            .transform.Cast<Transform>()
            .Where(c => c.gameObject.tag == "Spawn")
            .ToArray();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bgImg.rectTransform,
            ped.position,
            ped.pressEventCamera,
            out pos
        ) && nextFire < Time.time) {
            GetComponent<AudioSource>().Play();
            nextFire = Time.time + fireRate;
            float delay = 0.02F;

            foreach (Transform shotSpawn in arrayOfChildren) {
                GameObject clone =
                    Instantiate(
                        shot,
                        shotSpawn.position,
                        shotSpawn.rotation
                    ) as GameObject; // we instantiate as GameObject to refer it afterwards		
//										yield return new WaitForSeconds(delay);
                delay = 0;
            }
        }
    }
}