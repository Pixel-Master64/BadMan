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

    enum HandState { Idle, Attacking, Hold, Smashing, Slide, Sliding, SlidingReturn, Projectile, Fire, Weak, Dead, Dying, Wait };
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
            case HandState.Idle:
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
            case HandState.Attacking:
                {
                    currentlyAttacking = true;
                    enableHitbox = false;

                    float distanceToTarget;

                    distanceToTarget = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(player.transform.position.x, 1.5f));
                    timePassed += Time.deltaTime;

                    attackTarget = new Vector2(player.transform.position.x, 1.5f);
                    transform.position = Vector2.MoveTowards(transform.position, attackTarget, distanceToTarget / 0.70f * Time.deltaTime * attackSpeed);

                    if (timePassed > 3)
                    {
                        timePassed = 0;
                        handState = HandState.Hold;

                    }
                    break;
                }
            case HandState.Hold:
                {
                    timePassed += Time.deltaTime;
                    if (timePassed > 0.4)
                    {
                        timePassed = 0;
                        handState = HandState.Smashing;
                    }
                        
                    break;
                }
            case HandState.Smashing:
                {
                    currentAttack = 1;
                    currentlyAttacking = true;
                    enableHitbox = true;

                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, -3.5f), 12.5f * Time.deltaTime * attackSpeed);

                    if (Vector2.Distance(transform.position, new Vector2(transform.position.x, -3.5f)) < 0.001f)
                    {
                        timePassed += Time.deltaTime;
                        enableHitbox = false;
                    }

                    if (timePassed > 1.5f * timerSpeed)
                    {
                        timePassed = 0;
                        handState = HandState.Idle;
                        boss.GetComponent<BossAttacks>().attacksUntillWeak--;
                    }

                    break;
                }
            case HandState.Slide:
                {
                    currentlyAttacking = true;
                    enableHitbox = false;

                    float distanceToTarget;
                    if (LeftHand)
                    {
                        distanceToTarget = Vector2.Distance(transform.position, new Vector2(-10.5f, -5f));
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-10.5f, -5f), distanceToTarget / 0.75f * Time.deltaTime * attackSpeed);
                    }
                    else
                    {
                        distanceToTarget = Vector2.Distance(transform.position, new Vector2(10.5f, -5f));
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(10.5f, -5f), distanceToTarget / 0.75f * Time.deltaTime * attackSpeed);
                    }

                    timePassed += Time.deltaTime;

                    if (timePassed > 3.5f * timerSpeed)
                    {
                        timePassed = 0;
                        handState = HandState.Sliding;
                    }

                    break;
                }
            case HandState.Sliding:
                {
                    currentAttack = 2;
                    currentlyAttacking = true;
                    enableHitbox = true;

                    if (LeftHand)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(13f, transform.position.y), 15f * Time.deltaTime * attackSpeed);
                        if (Vector2.Distance(transform.position, new Vector2(13f, transform.position.y)) < 0.1f)
                        {
                            timePassed += Time.deltaTime;
                            enableHitbox = false;
                        }
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-13f, transform.position.y), 15f * Time.deltaTime * attackSpeed);
                        if (Vector2.Distance(transform.position, new Vector2(-13f, transform.position.y)) < 0.1f)
                        {
                            timePassed += Time.deltaTime;
                            enableHitbox = false;
                        }
                    }

                    if (timePassed > 1.5f * timerSpeed)
                    {
                        timePassed = 0;
                        handState = HandState.SlidingReturn;
                        boss.GetComponent<BossAttacks>().attacksUntillWeak--;
                    }

                    break;
                }
            case HandState.SlidingReturn:
                {
                    currentlyAttacking = true;
                    enableHitbox = false;

                    timePassed += Time.deltaTime;

                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x / 1.5f, 3), 18f * Time.deltaTime * attackSpeed);

                    if (timePassed > 0.9 * timerSpeed)
                    {
                        timePassed = 0;
                        handState = HandState.Idle;
                    }

                    break;
                }
            case HandState.Projectile:
                {
                    if (LeftHand)
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-7 + Mathf.Sin(gameManager.gameTimer * 5 * attackSpeed), 1 + Mathf.Cos(gameManager.gameTimer * 5 * attackSpeed)), 10f * Time.deltaTime * attackSpeed);
                    else
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(7 + Mathf.Sin(gameManager.gameTimer * 5 * attackSpeed), 1 + Mathf.Cos(gameManager.gameTimer * 5 * attackSpeed)), 10f * Time.deltaTime * attackSpeed);

                    timePassed += Time.deltaTime;

                    if (timePassed > 1.5 * timerSpeed)
                    {
                        timePassed = 0;
                        GameObject Projectile = (GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation);

                        handState = HandState.Idle;
                        boss.GetComponent<BossAttacks>().attacksUntillWeak--;
                    }
                    break;
                }
            case HandState.Weak:
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
            case HandState.Dead:
                {
                    if (LeftHand)
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-7 + Mathf.Sin(gameManager.gameTimer * 20), 1 + Mathf.Cos(gameManager.gameTimer * 20)), 10f * Time.deltaTime);
                    else
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(7 + Mathf.Sin(gameManager.gameTimer * 20), 1 + Mathf.Cos(gameManager.gameTimer * 20)), 10f * Time.deltaTime);

                    break;
                }
            case HandState.Dying:
                {
                    timePassed += Time.deltaTime;

                    if (LeftHand)
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-7 + Mathf.Sin(gameManager.gameTimer * 20), 1 + Mathf.Cos(gameManager.gameTimer * 20)), 10f * Time.deltaTime);
                    else
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(7 + Mathf.Sin(gameManager.gameTimer * 20), 1 + Mathf.Cos(gameManager.gameTimer * 20)), 10f * Time.deltaTime);

                    gameObject.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                    if (timePassed > 3)
                    {
                        
                            Destroy(gameObject);
                    }
                    break;
                }

        }
    }

    public void Startattack()
    {
        if (!currentlyAttacking)
        {
            int attack = Random.Range(1, 4);
            timePassed = 0;
            if (attack == 1)
                handState = HandState.Attacking;
            else if (attack == 2)
                handState = HandState.Slide;
            else if (attack == 3)
                handState = HandState.Projectile;
        }
    }
    public void BeWeak()
    {
        if (!currentlyAttacking)
            handState = HandState.Weak;
    }
    public void BeNormal()
    {
        handState = HandState.Idle;
    }
    public void BeDead()
    {
        handState = HandState.Dead;
    }
    public void BeDying()
    {
        handState = HandState.Dying;
    }
}

