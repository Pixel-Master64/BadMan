using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Platformer
{
    public class PlayerDeathState : MonoBehaviour
    {
        public float jumpForce;
        public float sideForce;

        private Rigidbody2D rigidbody;
        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            rigidbody.AddForce(transform.right * sideForce, ForceMode2D.Impulse);
        }
    }
}

