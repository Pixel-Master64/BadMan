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

    bool justAttacked;

    

    // Start is called before the first frame update
    void Start()
    {
        justAttacked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.InCutscene)
        {
            if(GameManager.totalSeconds % 4 == 3 && !justAttacked)
            {
                justAttacked = true;
                Debug.Log("Attack >:(");

                if(Random.Range(0,2) == 1)
                    rightHand.GetComponent<BossHandMove>().Startattack();
                else
                    leftHand.GetComponent<BossHandMove>().Startattack();
            }
        }

        if(justAttacked && GameManager.totalSeconds % 4 != 3)
            justAttacked=false;
    }
}
