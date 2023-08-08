using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBackgroundParticles : MonoBehaviour
{
    public GameObject Boss;
    public int bossHealth;
    public float particleHeight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bossHealth = Boss.gameObject.GetComponent<Health>().CurrentHealth;
        if (bossHealth > 0)
            particleHeight = (float)(-26 + (6 * (6 - bossHealth-1)));
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,particleHeight,transform.position.z), 2 * Time.deltaTime);
    }
}
