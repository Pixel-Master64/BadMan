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

        if (Hand.GetComponent<BossHandMove>().currentAttack == 1)
        {
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -0.5f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.95f, 0.05f);
        }
        else if (Hand.GetComponent<BossHandMove>().currentAttack == 2)
        {
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        }
    }
}
