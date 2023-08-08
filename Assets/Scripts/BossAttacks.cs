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

    enum BossState {Idle, Weak};
    BossState bossState;

    // Start is called before the first frame update
    void Start()
    {
        attacksUntillWeak = 5;
        justAttacked = false;
        bossState = BossState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (bossState)
        {
            case BossState.Idle:
                {
                    gameObject.GetComponent<Boss>().readyToBeHit = false;

                    if (GetComponent<Health>().CurrentHealth == 6) //this code is really lame im sorry
                        speed = 1f;
                    else if (GetComponent<Health>().CurrentHealth == 5)
                        speed = 1.35f;
                    else if (GetComponent<Health>().CurrentHealth == 4)
                        speed = 1.7f;
                    else if (GetComponent<Health>().CurrentHealth == 3)
                        speed = 2f;
                    else if (GetComponent<Health>().CurrentHealth == 2)
                        speed = 2.35f;
                    else if (GetComponent<Health>().CurrentHealth == 1)
                        speed = 3f;
                    else
                        speed = 1f;

                    attackSpeed = Mathf.FloorToInt(speed)-1;

                    if (!GameManager.InCutscene)
                    {
                        if (GameManager.totalSeconds % (4 - attackSpeed) == 0 && !justAttacked)
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

                    if (justAttacked && GameManager.totalSeconds % (4 - attackSpeed) != 0)
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
                        attacksUntillWeak = 6 + attackSpeed;
                        timePassed = 0;
                        bossState = BossState.Idle;
                        gameObject.GetComponent<Boss>().readyToBeHit = false;
                        rightHand.GetComponent<BossHandMove>().BeNormal();
                        leftHand.GetComponent<BossHandMove>().BeNormal();

                    }
                    break;
                }
        }
    }

    public void GoBackToNormal()
    {
        attacksUntillWeak = 6;
        bossState = BossState.Idle;
        rightHand.GetComponent<BossHandMove>().BeNormal();
        leftHand.GetComponent<BossHandMove>().BeNormal();
    }

}
