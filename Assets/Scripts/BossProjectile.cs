using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public GameObject target;
    public Vector2 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        targetPosition = target.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 4f*Time.deltaTime);
        if ((Vector2.Distance(transform.position, new Vector2(targetPosition.x, targetPosition.y)) < 0.01f))
        {
            Destroy(gameObject);
        }
    }
}
