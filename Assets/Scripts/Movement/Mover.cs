﻿using UnityEngine;

namespace Movement {
    public class Mover : MonoBehaviour {
        public float speed;

        private void Start() {
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }

        private void Update() { }
    }
}