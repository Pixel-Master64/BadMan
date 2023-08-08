using Platformer;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class BossHeadMove : MonoBehaviour
{
    public GameManager gameManager;
    public Vector3 StartingLocation;
    public float xIdleIntensity;
    public float yIdleIntensity;
    public float xCycleIntensity;
    public float yCycleIntensity;

    // Start is called before the first frame update
    void Start()
    {
        StartingLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(StartingLocation.x + (Mathf.Cos(gameManager.gameTimer * xCycleIntensity) * xIdleIntensity), StartingLocation.y + (Mathf.Cos(gameManager.gameTimer * yCycleIntensity) * yIdleIntensity),5);
    }
}
