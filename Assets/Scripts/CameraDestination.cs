using Platformer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Xml.XPath;
using Unity.VisualScripting;
using UnityEngine;

public class CameraDestination : MonoBehaviour
{
    public GameObject Player;
    public float Amplification;
    public bool facingRight;
    public Vector2 Positon;
    public Vector2 Speed;

    // Start is called before the first frame update
    void Start()
    {
        //transform.SetParent(null, true);
    }

    // Update is called once per frame
    void Update()
    {
        Positon = Player.gameObject.GetComponent<PlayerController>().CurrentPositon;
        Speed = Player.gameObject.GetComponent<PlayerController>().CurrentSpeed;
        facingRight = Player.gameObject.GetComponent<PlayerController>().facingRight;

        if (Speed.x > 0.2)
            Speed.x = 0.2f;
        if (Speed.x < -0.2)
            Speed.x = -0.2f;

        if (Speed.y > 0.15)
            Speed.y = 0.15f;
        if (Speed.y < -0.15)
            Speed.y = -0.15f;


        Vector2 target;
        target = new Vector2(Positon.x + (Speed.x * Amplification * -4f), Positon.y + (Speed.y * Amplification * -3f));
        
           
                
        float step = Vector2.Distance(transform.position, target)/350;

        transform.position = Vector2.MoveTowards(transform.position, target, step);

    }
}
