using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float xAngle, yAngle, zAngle;
    public float xSpin, ySpin, zSpin;
    public GameManager GameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xAngle = transform.rotation.x;
        yAngle = transform.rotation.y;
        zAngle = transform.rotation.z;
        //transform.Rotate(xSpin, ySpin, zSpin*(Mathf.Pow(Mathf.Cos(GameManager.gameTimer),2))/Mathf.PI, Space.Self);
        transform.Rotate(xSpin, ySpin, zSpin, Space.Self);
    }
}
