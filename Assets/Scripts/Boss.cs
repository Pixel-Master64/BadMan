using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Boss : MonoBehaviour
{
    public Vector3 restPosition;
    public Vector3 attackPosition;
    public float xAngle, yAngle, zAngle;
    public GameObject Player;
    public BoxCollider2D Collider;
    public BoxCollider2D HurtBox;

    public bool readyToBeHit = false;

    public AudioSource sfxPlayer;
    // Start is called before the first frame update
    void Start()
    {
        restPosition = transform.position;
        attackPosition = new Vector3(restPosition.x,restPosition.y,restPosition.z-3);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerPos = (Player.transform.position).normalized;
        xAngle = transform.rotation.eulerAngles.x;
        yAngle = transform.rotation.eulerAngles.y;
        zAngle = transform.rotation.eulerAngles.z;
        transform.LookAt(PlayerPos);

        float distacneFromAttackPos = Vector3.Distance(transform.position, attackPosition);
        float distacneFromRestPos = Vector3.Distance(transform.position, restPosition);

        if (readyToBeHit)
        {
            transform.position = Vector3.MoveTowards(transform.position, attackPosition, distacneFromAttackPos / 0.25f * Time.deltaTime);
            if(distacneFromAttackPos<1)
            {
                Collider.enabled = true;
                HurtBox.enabled = true;
                GetComponent<Health>().CanBeHurt = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, restPosition, distacneFromRestPos / 0.25f * Time.deltaTime);
            Collider.enabled = false;
            HurtBox.enabled = false;
            GetComponent<Health>().CanBeHurt = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //take damage
            GetComponent<Health>().CurrentHealth = GetComponent<Health>().CurrentHealth -1;
            readyToBeHit = false;
            //Fling Bad Man
            other.rigidbody.AddForce(transform.up * -2, ForceMode2D.Impulse);

            sfxPlayer.Play();

            //return to normal state
            GetComponent<BossAttacks>().GoBackToNormal();
        }
    }
}
