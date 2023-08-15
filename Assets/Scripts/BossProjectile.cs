using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BossProjectile : MonoBehaviour
{
    public GameObject target;
    public GameObject boss;
    public Vector2 targetPosition;
    public Vector2 targetLookAt;
    public Vector2 raycastPosition;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        boss = GameObject.Find("Boss Head");

        targetPosition = target.transform.position;
        targetLookAt = (target.transform.position - transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 3;
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, targetLookAt, Mathf.Infinity, layerMask);
        raycastPosition = hit.point;

        float speed = boss.GetComponent<BossAttacks>().speed;

        transform.position = Vector2.MoveTowards(transform.position, raycastPosition, 4f * Time.deltaTime * speed);

        if ((Vector2.Distance(transform.position, raycastPosition) < 0.001f))
        {
            Destroy(gameObject);
        }


    }
}
