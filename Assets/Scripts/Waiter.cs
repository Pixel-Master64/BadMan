using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waiter : MonoBehaviour
{
    float timePassed;
    enum HandState { Idle, Wait };
    HandState handState;

    // Start is called before the first frame update
    void Start()
    {
        handState = HandState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (handState)
        {
            case HandState.Idle:
                {
                    break;
                }
            case HandState.Wait:
                {
                    timePassed += Time.deltaTime;
                    if (timePassed > 4)
                    {
                        SceneManager.LoadScene("Winner");
                    }
                    break;
                }
        }
    }

    public void StartWait()
    {
        timePassed = 0;
        handState = HandState.Wait;
    }
}
