using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject Target;
    public Vector2 Offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetLocation = Target.transform.position;
        transform.position = new Vector2(targetLocation.x + Offset.x, targetLocation.y + Offset.y);
    }
}
