using Platformer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool ActullyDoHealthAndStuff;
    public PlayerController playerController;
    public int MaxHealth;
    public int CurrentHealth;
    public float Iframes;
    float timePassed;
    public bool CanBeHurt;

    public GameManager gameManager;

    public GameObject owner;

    public SpriteRenderer spriteRenderer;


    enum HealthState { canBeHurt, invincable };
    HealthState healthState;

    // Start is called before the first frame update
    void Start()
    {

        if (ActullyDoHealthAndStuff)
            CurrentHealth = MaxHealth;
        else
            enabled = false;

        healthState = HealthState.canBeHurt;

        owner = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHealth < 1 && playerController.deathState == false)
            playerController.deathState = true;

        switch (healthState)
        {
            case HealthState.canBeHurt:
                {
                    CanBeHurt = true;
                    return;
                }
            case HealthState.invincable:
                {
                    spriteRenderer.color = Color.grey;

                    timePassed += Time.deltaTime;

                    if (timePassed > Iframes / 60)
                    {
                        spriteRenderer.color = Color.white;
                        healthState = HealthState.canBeHurt;
                        timePassed = 0;
                    }
                    return;
                }
        }

    }

    public void Hurt()
    {
        if (CanBeHurt)
        {
            timePassed = 0;
            CurrentHealth = CurrentHealth - 1;
            CanBeHurt = false;
            healthState = HealthState.invincable;

            //if(owner.name == "Player")
            //{
            //    owner.GetComponent<Rigidbody2D>().AddForce(transform.up * 6, ForceMode2D.Impulse);
            //}
        }
    }
}