using Platformer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public GameManager GameManager;
    public int seconds;

    public int attacksUntillWeak;

    bool justAttacked;

    float timePassed;

    public float speed;
    public int attackSpeed;

    public GameObject Temp;
    public GameObject Temp2;

    bool initialWait;



    enum BossState { Wait, Idle, Weak, Dead, Dying };
    BossState bossState;

    // Start is called before the first frame update
    void Start()
    {
        attacksUntillWeak = 5;
        justAttacked = false;
        initialWait = false;
        timePassed = 0;
        bossState = BossState.Wait;
    }

    // Update is called once per frame
    void Update()
    {
        switch (bossState)
        {
            case BossState.Wait:
                {
                    if (!GameManager.InCutscene)
                    {
                        timePassed += Time.deltaTime;

                        if (timePassed > 2)
                        {
                            bossState = BossState.Idle;
                            timePassed = 0;
                        }
                    }
                    break;
                }
            case BossState.Idle:
                {
                    gameObject.GetComponent<Boss>().readyToBeHit = false;

                    if (GetComponent<Health>().CurrentHealth == 6) //this code is really lame im sorry
                        speed = 1f;
                    else if (GetComponent<Health>().CurrentHealth == 5)
                        speed = 1.25f;
                    else if (GetComponent<Health>().CurrentHealth == 4)
                        speed = 1.5f;
                    else if (GetComponent<Health>().CurrentHealth == 3)
                        speed = 1.75f;
                    else if (GetComponent<Health>().CurrentHealth == 2)
                        speed = 2f;
                    else if (GetComponent<Health>().CurrentHealth == 1)
                        speed = 2.35f;
                    else
                        speed = 1f;

                    attackSpeed = Mathf.FloorToInt(speed) - 1;

                    if (!GameManager.InCutscene)
                    {
                        if ((GameManager.totalSeconds % (4 - attackSpeed) == 0 && !justAttacked)) // || GetComponent<Health>().CurrentHealth == 1
                        {
                            justAttacked = true;
                            Debug.Log("Attack >:(");

                            if (Random.Range(0, 2) == 1)
                            {
                                if (!rightHand.GetComponent<BossHandMove>().currentlyAttacking)
                                    rightHand.GetComponent<BossHandMove>().Startattack();
                                else
                                    leftHand.GetComponent<BossHandMove>().Startattack();
                            }

                            else
                            {
                                if (!leftHand.GetComponent<BossHandMove>().currentlyAttacking)
                                    leftHand.GetComponent<BossHandMove>().Startattack();
                                else
                                    rightHand.GetComponent<BossHandMove>().Startattack();
                            }

                            if (attacksUntillWeak < 1)
                            {
                                timePassed = 0;
                                rightHand.GetComponent<BossHandMove>().BeWeak();
                                leftHand.GetComponent<BossHandMove>().BeWeak();
                                bossState = BossState.Weak;
                            }
                        }
                    }

                    if (GetComponent<Health>().CurrentHealth != 1)
                    {
                        if (justAttacked && GameManager.totalSeconds % (4 - attackSpeed) != 0)
                            justAttacked = false;
                    }
                    else                                        //when on 1HP, GO NUTS
                        justAttacked = false;

                    break;
                }
            case BossState.Weak:
                {
                    gameObject.GetComponent<Boss>().readyToBeHit = true;

                    rightHand.GetComponent<BossHandMove>().BeWeak();
                    leftHand.GetComponent<BossHandMove>().BeWeak();

                    timePassed += Time.deltaTime;

                    if (timePassed > 10)
                    {
                        attacksUntillWeak = 6;
                        timePassed = 0;
                        bossState = BossState.Idle;
                        gameObject.GetComponent<Boss>().readyToBeHit = false;
                        rightHand.GetComponent<BossHandMove>().BeNormal();
                        leftHand.GetComponent<BossHandMove>().BeNormal();

                    }
                    break;
                }
            case BossState.Dead:
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(Mathf.Sin(GameManager.gameTimer * 20), Mathf.Cos(GameManager.gameTimer * 20)), 10f * Time.deltaTime);
                    break;
                }
            case BossState.Dying:
                {
                    timePassed += Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(Mathf.Sin(GameManager.gameTimer * 20), Mathf.Cos(GameManager.gameTimer * 20)), 10f * Time.deltaTime);
                    gameObject.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                    if (timePassed > 3)
                    {
                        Destroy(gameObject);
                    }
                    break;
                }
        }
    }

    public void GoBackToNormal()
    {
        if (GetComponent<Health>().CurrentHealth != 0)
        {
            attacksUntillWeak = 6 + Mathf.FloorToInt(speed);
            if (GetComponent<Health>().CurrentHealth == 1)
                attacksUntillWeak = 12;
            bossState = BossState.Idle;
            rightHand.GetComponent<BossHandMove>().BeNormal();
            leftHand.GetComponent<BossHandMove>().BeNormal();
        }
        else
        {
            rightHand.GetComponent<BossHandMove>().BeDead();
            leftHand.GetComponent<BossHandMove>().BeDead();
            bossState = BossState.Dead;

            Temp.GetComponent<TriggerData>().MoveDown();
        }
    }
    public void DieFully()
    {
        Temp2.GetComponent<Waiter>().StartWait();

        timePassed = 0;
        rightHand.GetComponent<BossHandMove>().BeDying();
        leftHand.GetComponent<BossHandMove>().BeDying();
        bossState = BossState.Dying;
        GameManager.BossDied();
    }
}
