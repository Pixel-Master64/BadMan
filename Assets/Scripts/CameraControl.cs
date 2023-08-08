using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraControl : MonoBehaviour
{
    public GameObject Player;
    public float speed;
    public Vector2 Target;
    public float damping;
    public float Zaxis;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Target = Player.GetComponent<PlayerController>().CameraDestinationChild;
        
        float distance = Vector2.Distance(transform.position, Target);
        float step = (speed * distance)/damping;

        transform.position = Vector2.MoveTowards(transform.position, Target, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, Zaxis);
    }
}
