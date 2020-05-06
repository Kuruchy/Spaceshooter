using UnityEngine;

namespace Core {
    public class BgScroller : MonoBehaviour {
        [SerializeField] private float scrollSpeed;

        private Material nebulaMaterial;
        private Vector2 offset;

        private void Awake() {
            nebulaMaterial = GetComponent<Renderer>().material;
            offset = new Vector2(0f, scrollSpeed);
        }

        private void Update() {
            nebulaMaterial.mainTextureOffset += offset * Time.deltaTime;
        }
    }
}