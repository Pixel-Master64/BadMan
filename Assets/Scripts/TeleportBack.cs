using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(other.transform.position.x < -10)
            {
                other.transform.position = new Vector2(-10, -4.3f);
            }
            else if (other.transform.position.x > 10)
            {
                other.transform.position = new Vector2(10, -4.3f);
            }
            else
            {
                other.transform.position = new Vector2(other.transform.position.x, -4.3f);
            }
        }
    }
}
