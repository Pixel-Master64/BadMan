using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject child;
    public Vector2 startPos;
    public Vector2 endPos;
    public int Speed;
    public bool GoingForward = true;

    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.position;
        endPos = child.transform.position;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GoingForward == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPos, Speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, endPos) < 0.001f)
            {
                GoingForward = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos, Speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, startPos) < 0.001f)
            {
                GoingForward = true;
            }
        }
    }
}
