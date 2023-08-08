using Platformer;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
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

    public bool currentlyAttacking;
    public bool enableHitbox;

    float timePassed;

    Vector2 attackTarget;

    enum HandState { Idle, Attacking, Smashing, Slide, Sliding, Projectile, Fire};
    HandState handState;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        StartingLocation = transform.position;
        currentlyAttacking = false;

        handState = HandState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
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
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(StartingLocation.x, StartingLocation.y + (Mathf.Sin(gameManager.gameTimer * speed) * IdleIntensity)), distanceToTarget / 0.5f * Time.deltaTime);
                    }
                    else
                    {
                        distanceToTarget = Vector2.Distance(transform.position, new Vector2(StartingLocation.x, StartingLocation.y + (Mathf.Cos(gameManager.gameTimer * speed) * IdleIntensity)));
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(StartingLocation.x, StartingLocation.y + (Mathf.Cos(gameManager.gameTimer * speed) * IdleIntensity)), distanceToTarget / 0.5f * Time.deltaTime);
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
                    transform.position = Vector2.MoveTowards(transform.position, attackTarget, distanceToTarget / 0.70f * Time.deltaTime);

                    if (timePassed > 3)
                    {
                        timePassed = 0;
                        handState = HandState.Smashing;

                    }
                    break;
                }
            case HandState.Smashing:
                {
                    currentlyAttacking = true;
                    enableHitbox = true;

                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, -3.5f), 12.5f * Time.deltaTime);

                    if (Vector2.Distance(transform.position, new Vector2(transform.position.x, -3.5f)) < 0.001f)
                    {
                        timePassed += Time.deltaTime;
                        enableHitbox = false;
                    }

                    if (timePassed > 1.5)
                    {
                        timePassed = 0;
                        handState = HandState.Idle;
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
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-10.5f, -5f), distanceToTarget / 0.75f * Time.deltaTime);
                    }
                    else
                    {
                        distanceToTarget = Vector2.Distance(transform.position, new Vector2(10.5f, -5f));
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(10.5f, -5f), distanceToTarget / 0.75f * Time.deltaTime);
                    }

                    timePassed += Time.deltaTime;

                    if (timePassed > 3.5f)
                    {
                        timePassed = 0;
                        handState = HandState.Sliding;
                    }

                    break;
                }
            case HandState.Sliding:
                {
                    currentlyAttacking = true;
                    enableHitbox = true;

                    if (LeftHand)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(10.5f, transform.position.y), 20f * Time.deltaTime);
                        if (Vector2.Distance(transform.position, new Vector2(10.5f, transform.position.y)) < 0.001f)
                        {
                            timePassed += Time.deltaTime;
                            enableHitbox = false;
                        }
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-10.5f, transform.position.y), 20f * Time.deltaTime);
                        if (Vector2.Distance(transform.position, new Vector2(-10.5f, transform.position.y)) < 0.001f)
                        {
                            timePassed += Time.deltaTime;
                            enableHitbox = false;
                        }
                    }

                    if (timePassed > 1.5)
                    {
                        timePassed = 0;
                        handState = HandState.Idle;
                    }

                    break;
                }
        }
    }

    public void Startattack()
    {
        if(!currentlyAttacking)
        {
            timePassed = 0;
            handState = HandState.Slide;
        }
    }
}