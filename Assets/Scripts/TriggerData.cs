using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerData : MonoBehaviour
{
    public bool TriggerTypeTextBox;
    public string[] Text = { "Hi! this is a placeholder", "Zachary took a snoozer and forgot to add text!", "Hah! What a dingus!" };
    public string[] Speakers = { "Bad Man", "Bad Man", "Bad Man" };

    public bool TriggerTypeWarp;
    public string destination;

    public bool TriggerTypeChangeChallange;
    public int Challange;

    public bool TriggerTypeLaunch = false;
    public int LaunchForceRight;
    public int LaunchForceUp;

    public bool actionAfterClose;
    public int actionID;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveDown()
    {
        gameObject.transform.position = new Vector2(transform.position.x, -4.5f);
    }
}
