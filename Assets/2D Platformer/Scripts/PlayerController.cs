using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private float moveInput;

        public bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;

        public bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private GameManager gameManager;

        public bool HasJumped = false;

        public Vector2 CurrentPositon;
        public Vector2 CurrentSpeed;
        public GameObject CameraDestination;
        public Vector2 CameraDestinationChild;

        public AudioSource JumpSfxPlayer;
        public AudioSource MiscSfxPlayer;
        public AudioClip CoinSfx;
        public AudioClip KeySfx;
        public AudioClip UnlockSfx;
        public AudioClip SpringSfx;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void FixedUpdate()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            transform.Rotate(0, 0, 0);

            CheckGround();

            if (gameManager.InCutscene == false)
            {
                if (Input.GetButton("Horizontal"))
                {
                    moveInput = Input.GetAxis("Horizontal");
                    Vector3 direction = transform.right * moveInput;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                }
                if (facingRight == false && moveInput > 0)
                {
                    Flip();
                }
                else if (facingRight == true && moveInput < 0)
                {
                    Flip();
                }
            }

            CurrentSpeed = new Vector2(CurrentPositon.x - transform.position.x, CurrentPositon.y - transform.position.y); // For the Camera Destination Child Object
            CurrentPositon = transform.position;
            CameraDestinationChild = CameraDestination.transform.position;
        }

        void Update()
        {
            if (gameManager.InCutscene == false)
            {
                if (Input.GetKeyDown(KeyCode.W) && isGrounded)
                {
                    rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                    HasJumped = true;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    gameManager.advanceTextBox();
                }
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;

            if (facingRight)
                transform.GetComponent<SpriteRenderer>().flipX = true;
            else
                transform.GetComponent<SpriteRenderer>().flipX = false;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
            if (colliders.Length > 0.01)
                HasJumped = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                if (GetComponent<Health>().ActullyDoHealthAndStuff == false) // check if we doing the health system
                    deathState = true; // Say to GameManager that player is dead
                else
                    GetComponent<Health>().Hurt();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
            }

            if (other.gameObject.tag == "Key")
            {
                gameManager.keysCounter += 1;
                Destroy(other.gameObject);
            }

            if (other.gameObject.tag == "Lock")
            {
                if (gameManager.keysCounter > 0)
                {
                    gameManager.keysCounter -= 1;
                    Destroy(other.gameObject);
                }
            }

            if (other.gameObject.tag == "JumpPad")
            {
                rigidbody.AddForce(transform.up * 12, ForceMode2D.Impulse);
            }
            if (other.gameObject.tag == "SuperJumpPad")
            {
                rigidbody.AddForce(transform.up * 20, ForceMode2D.Impulse);
            }

            if (other.gameObject.tag == "Trigger") //Triggers
            {
                if (other.gameObject.GetComponent<TriggerData>().TriggerTypeTextBox == true) //Text Box
                {
                    gameManager.TextBoxDialog = other.gameObject.GetComponent<TriggerData>().Text;
                    gameManager.TextBoxSpeakers = other.gameObject.GetComponent<TriggerData>().Speakers;

                    if (other.gameObject.GetComponent<TriggerData>().actionAfterClose)
                        gameManager.ActionAfterDialog = other.gameObject.GetComponent<TriggerData>().actionID;
                    else
                        gameManager.ActionAfterDialog = 0;

                    gameManager.LoadDialog();
                    gameManager.InCutscene = true;
                    
                }
                if (other.gameObject.GetComponent<TriggerData>().TriggerTypeChangeChallange == true) // Change Challange
                {
                    gameManager.CurrentChallange = other.gameObject.GetComponent<TriggerData>().Challange;
                    if (other.gameObject.GetComponent<TriggerData>().Challange == 5)
                    {
                        gameManager.gameTimer = 0;
                    }

                }
                if (other.gameObject.GetComponent<TriggerData>().TriggerTypeWarp == true) // Change Scene
                {
                    SceneManager.LoadScene(other.gameObject.GetComponent<TriggerData>().destination);
                }
                if (other.gameObject.GetComponent<TriggerData>().TriggerTypeLaunch == true) // Change Scene
                {
                    rigidbody.AddForce(transform.right * (other.gameObject.GetComponent<TriggerData>().LaunchForceRight), ForceMode2D.Impulse);
                    rigidbody.AddForce(transform.up * (other.gameObject.GetComponent<TriggerData>().LaunchForceUp), ForceMode2D.Impulse);
                }
                Destroy(other.gameObject);
            }
        }

        private void OnTriggerStay2D(Collider2D other) //If on moving platform, follow it
        {

            if (other.gameObject.tag == "Moving Platform")
            {
                transform.parent = other.transform;

            }
        }

        private void OnTriggerExit2D(Collider2D other) //Leaves moving platform
        {
            if (other.gameObject.tag == "Moving Platform")
            {
                transform.parent = null;

            }
        }
    }
}
