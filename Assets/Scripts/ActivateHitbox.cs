using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ActivateHitbox : MonoBehaviour
{
    public GameObject Hand;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Hand.GetComponent<BossHandMove>().enableHitbox)
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        else
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
