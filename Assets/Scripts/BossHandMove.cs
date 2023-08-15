using Platformer;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BossHandMove : MonoBehaviour
{
    public GameManager gameManager;
    public bool LeftHand;
    public Vector2 StartingLocation;
    public float IdleIntensity;
    public float speed;

    public GameObject player;
    public GameObject hitbox;
    public GameObject boss;

    public bool currentlyAttacking;
    public bool enableHitbox;

    public int currentAttack;

    float timePassed;

    float attackSpeed;
    float timerSpeed;

    Vector2 attackTarget;

    public GameObject projectilePrefab;
    public MeshRenderer PLS;

    public float slidingHeight;

    enum HandState { Idle, Attacking, Hold, Smashing, Slide, Sliding, SlidingReturn, Projectile, Fire, Weak, Dead, Dying, Wait};
    HandState handState;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        boss = GameObject.Find("Boss Head");

        StartingLocation = transform.position;
        currentlyAttacking = false;

        handState = HandState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        attackSpeed = boss.GetComponent<BossAttacks>().speed;
        timerSpeed = 1 / attackSpeed;

        switch (handState)
        {
            case HandState.Idle: //Not Attacking, Move up and down in a sin/cos wave using the MoveTowards Function
                {
                    currentlyAttacking = false;
                    enableHitbox = false;

                    float distanceToTarget;
                    if (LeftHand)
                    {
                        distanceToTarget = Vector2.Distance(transform.position, new Vector2(StartingLocation.x, StartingLocation.y + (Mathf.Sin(gameManager.gameTimer * speed) * IdleIntensity)));
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(StartingLocation.x, StartingLocation.y + (Mathf.Sin(gameManager.gameTimer * speed) * IdleIntensity)), distanceToTarget / 0.5f * Time.deltaTime * attackSpeed);
                    }
                    else
                    {
                        distanceToTarget = Vector2.Distance(transform.position, new Vector2(StartingLocation.x, StartingLocation.y + (Mathf.Cos(gameManager.gameTimer * speed) * IdleIntensity)));
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(StartingLocation.x, StartingLocation.y + (Mathf.Cos(gameManager.gameTimer * speed) * IdleIntensity)), distanceToTarget / 0.5f * Time.deltaTime * attackSpeed);
                    }

                    break;

                }
            case HandState.Attacking: //Move and follow above the player
                {
                    currentlyAttacking = true;
                    enableHitbox = false;

                    float distanceToTarget;

                    distanceToTarget = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(player.transform.position.x, 1.5f));
                    timePassed += Time.deltaTime;

                    attackTarget = new Vector2(player.transform.position.x, 1.5f);
                    transform.position = Vector2.MoveTowards(transform.position, attackTarget, distanceToTarget / 0.70f * Time.deltaTime * attackSpeed);

                    if (timePassed > 3) //after 3 seconds, change state to "Hold"
                    {
                        timePassed = 0;
                        handState = HandState.Hold;

                    }
                    break;
                }
            case HandState.Hold: //Pause in mid-air to allow for the player to dodge
                {
                    timePassed += Time.deltaTime;
                    if (timePassed > 0.3) //after 0.3 seconds, change state to "Smashing"
                    {
                        timePassed = 0;
                        handState = HandState.Smashing;
                    }
                        
                    break;
                }
            case HandState.Smashing: //Move downwards to the floor, activating a hitbox that hurts the player
                {
                    currentAttack = 1;
                    currentlyAttacking = true;
                    enableHitbox = true;

                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, -3.5f), 12.5f * Time.deltaTime * attackSpeed);

                    if (Vector2.Distance(transform.position, new Vector2(transform.position.x, -3.5f)) < 0.001f) //after hitting the floor, turn of hitboxes, and start timer
                    {
                        timePassed += Time.deltaTime;
                        enableHitbox = false;
                    }

                    if (timePassed > 1.5f * timerSpeed) //after 1.5 seconds, change state to "Idle"
                    {
                        timePassed = 0;
                        handState = HandState.Idle;
                        boss.GetComponent<BossAttacks>().attacksUntillWeak--;
                    }

                    break;
                }
            case HandState.Slide: //Prepare slide by going towards the side of the stage using the MoveTowards function
                {
                        currentlyAttacking = true;
                    enableHitbox = false;

                    float distanceToTarget;
                    if (LeftHand)
                    {
                        distanceToTarget = Vector2.Distance(transform.position, new Vector2(-10.5f, slidingHeight));
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-10.5f, slidingHeight), distanceToTarget / 0.75f * Time.deltaTime * attackSpeed);
                    }
                    else
                    {
                        distanceToTarget = Vector2.Distance(transform.position, new Vector2(10.5f, slidingHeight));
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(10.5f, slidingHeight), distanceToTarget / 0.75f * Time.deltaTime * attackSpeed);
                    }

                    timePassed += Time.deltaTime;

                    if (timePassed > 3.5f * timerSpeed) //after 3.5 seconds, change state to "Sliding"
                    {
                        timePassed = 0;
                        handState = HandState.Sliding;
                    }

                    break;
                }
            case HandState.Sliding: //Rapidly move to the other side of the screen, activating a hitbox that hurts the player
                {
                    currentAttack = 2;
                    currentlyAttacking = true;
                    enableHitbox = true;

                    if (LeftHand)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(13f, transform.position.y), 15f * Time.deltaTime * attackSpeed);
                        if (Vector2.Distance(transform.position, new Vector2(13f, transform.position.y)) < 0.1f) //after reaching the destination, turn of hitboxes and start timer
                        {
                            timePassed += Time.deltaTime;
                            enableHitbox = false;
                        }
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-13f, transform.position.y), 15f * Time.deltaTime * attackSpeed);
                        if (Vector2.Distance(transform.position, new Vector2(-13f, transform.position.y)) < 0.1f) //after reaching the destination, turn of hitboxes and start timer
                        {
                            timePassed += Time.deltaTime;
                            enableHitbox = false;
                        }
                    }

                    if (timePassed > 1.5f * timerSpeed) //after 1.5 seconds, change state to "SlidingReturn"
                    {
                        timePassed = 0;
                        handState = HandState.SlidingReturn;
                        boss.GetComponent<BossAttacks>().attacksUntillWeak--;
                    }

                    break;
                }
            case HandState.SlidingReturn: //Moves up and over across the stage so that it doesn't interfear with the player
                {
                    currentlyAttacking = true;
                    enableHitbox = false;

                    timePassed += Time.deltaTime;

                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x / 1.5f, 3), 18f * Time.deltaTime * attackSpeed);

                    if (timePassed > 0.9 * timerSpeed) //after 0.9 seconds, change state to "Idle"
                    {
                        timePassed = 0;
                        handState = HandState.Idle;
                    }

                    break;
                }
            case HandState.Projectile: //Spin around in a circle until timer is up, firing a projectile towards the player
                {
                    if (LeftHand)
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-7 + Mathf.Sin(gameManager.gameTimer * 5 * attackSpeed), 1 + Mathf.Cos(gameManager.gameTimer * 5 * attackSpeed)), 10f * Time.deltaTime * attackSpeed);
                    else
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(7 + Mathf.Sin(gameManager.gameTimer * 5 * attackSpeed), 1 + Mathf.Cos(gameManager.gameTimer * 5 * attackSpeed)), 10f * Time.deltaTime * attackSpeed);

                    timePassed += Time.deltaTime;

                    if (timePassed > 1.5f * ((timerSpeed*2)/3)) //after 1.5 seconds, create a Projectile object, change state to "Idle"
                    {
                        timePassed = 0;
                        GameObject Projectile = (GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation);

                        handState = HandState.Idle;
                        boss.GetComponent<BossAttacks>().attacksUntillWeak--;
                    }
                    break;
                }
            case HandState.Weak: // Hands hang down lower to allow the player to stand on them and reach the boss head to damage it
                {
                    currentlyAttacking = false;
                    enableHitbox = false;

                    float distanceToTarget;
                    if (LeftHand)
                    {
                        distanceToTarget = Vector2.Distance(transform.position, new Vector2(StartingLocation.x, StartingLocation.y - 5 + (Mathf.Sin(gameManager.gameTimer * speed) * (IdleIntensity + 2) + 2)));
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(StartingLocation.x, StartingLocation.y - 5 + (Mathf.Sin(gameManager.gameTimer * speed) * (IdleIntensity + 2) + 2)), distanceToTarget / 0.5f * Time.deltaTime);
                    }
                    else
                    {
                        distanceToTarget = Vector2.Distance(transform.position, new Vector2(StartingLocation.x, StartingLocation.y - 5 + (Mathf.Cos(gameManager.gameTimer * speed) * (IdleIntensity + 2) + 2)));
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(StartingLocation.x, StartingLocation.y - 5 + (Mathf.Cos(gameManager.gameTimer * speed) * (IdleIntensity + 2) + 2)), distanceToTarget / 0.5f * Time.deltaTime);
                    }

                    break;
                }
            case HandState.Dead: //Stop all functions and moves as if it is idle
                {
                    if (LeftHand)
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-7 + Mathf.Sin(gameManager.gameTimer * 20), 1 + Mathf.Cos(gameManager.gameTimer * 20)), 10f * Time.deltaTime);
                    else
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(7 + Mathf.Sin(gameManager.gameTimer * 20), 1 + Mathf.Cos(gameManager.gameTimer * 20)), 10f * Time.deltaTime);

                    break;
                }
            case HandState.Dying: //Spin around really fast while growing in size
                {
                    timePassed += Time.deltaTime;

                    if (LeftHand)
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-7 + Mathf.Sin(gameManager.gameTimer * 20), 1 + Mathf.Cos(gameManager.gameTimer * 20)), 10f * Time.deltaTime);
                    else
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(7 + Mathf.Sin(gameManager.gameTimer * 20), 1 + Mathf.Cos(gameManager.gameTimer * 20)), 10f * Time.deltaTime);

                    gameObject.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                    if (timePassed > 3) //after 3 seconds, destroy
                    {
                        
                            Destroy(gameObject);
                    }
                    break;
                }

        }
    }

    public void Startattack() //chose an attack though Random.range
    {
        if (!currentlyAttacking)
        {
            int attack = Random.Range(1, 4);
            timePassed = 0;
            if (attack == 1)
                handState = HandState.Attacking;
            else if (attack == 2)
            {
                if (Random.Range(1, 4) <= 2)
                    slidingHeight = -5f;
                else
                    slidingHeight = -2f;

                handState = HandState.Slide;
            }
                
            else if (attack == 3)
                handState = HandState.Projectile;
        }
    }
    public void BeWeak() //change state to "Weak"
    {
        if (!currentlyAttacking)
            handState = HandState.Weak;
    }
    public void BeNormal() //change state to "Idle"
    {
        handState = HandState.Idle;
    }
    public void BeDead() //change state to "Dead"
    {
        handState = HandState.Dead;
    }
    public void BeDying() //change state to "Dying"
    {
        handState = HandState.Dying;
    }
}

