using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EnemyAI : MonoBehaviour
    {
        public float moveSpeed = 1f; 
        public LayerMask ground;
        public LayerMask wall;

        private Rigidbody2D rigidbody; 
        public Collider2D triggerCollider;

        private GameManager gameManager;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        void Update()
        {
            if (gameManager.InCutscene == false)
            {
                rigidbody.velocity = new Vector2(moveSpeed, rigidbody.velocity.y);
            }
        }

        void FixedUpdate()
        {
            if(!triggerCollider.IsTouchingLayers(ground) || triggerCollider.IsTouchingLayers(wall))
            {
                Flip();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Lock")
            {
                Flip();
            }
        }



        private void Flip()
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            moveSpeed *= -1;
        }
    }
}
