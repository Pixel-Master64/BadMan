using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;

        public int keysCounter = 0;

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;

        public int CurrentChallange = 0;
        public Text CurrentChallangeUI;

        public Text CoinCounterUI;
        public Text KeyCounterUI;
        public Text ObjectiveUI;

        public Image TextBoxUI;
        public Text TextBoxText;
        public Text PressEnterToExitText;
        public Text TextBoxSpeaker;
        public bool InCutscene = false;

        public string[] TextBoxDialog;
        public string[] TextBoxSpeakers;
        public int DialogPointer;
        public int ActionAfterDialog;

        public string DeathMessage = "";
        public bool ShowDeathMessage = false;
        public Text DeathMessageText;

        public float gameTimer;
        public int totalSeconds;
        public int displaySeconds = 0;
        public int displayMinutes = 0;
        public int TimeLimit;
        public string timeDisplay;
        public Text TimerUI;
        public bool ShowTimer = false;

        float timePassed = 0;

        public GameObject musicPlayer;
        void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            musicPlayer = GameObject.Find("BG Music");
        }

        void Update()
        {
            //Timer Update stuff
            gameTimer += Time.deltaTime; //Tick up the gameTimer every frame by Time.deltaTime
            totalSeconds = Convert.ToInt32(Mathf.Floor(gameTimer)); //Rounds gameTimer down to seconds

            displaySeconds = (TimeLimit - totalSeconds) % 60; // set indavidual seconds
            displayMinutes = ((TimeLimit - totalSeconds) / 60) % 60; // set indavidual minutes

            string leadingZeroSeconds = ""; //Sets leading zeros
            if (displaySeconds < 10)
                leadingZeroSeconds = "0";
            timeDisplay = displayMinutes + ":" + leadingZeroSeconds + displaySeconds; //sets display timer string



            CoinCounterUI.text = "$" + coinsCounter.ToString(); //UI Update
            KeyCounterUI.text = keysCounter.ToString();
            ObjectiveUI.text = "Objective: ";

            if (InCutscene == true)          //Textbox show script
            {
                TextBoxUI.enabled = true;
                TextBoxText.enabled = true;
                TextBoxSpeaker.enabled = true;
                PressEnterToExitText.enabled = true;
            }

            else
            {
                TextBoxUI.enabled = false;
                TextBoxText.enabled = false;
                TextBoxSpeaker.enabled = false;
                PressEnterToExitText.enabled = false;
            }

            if (player.deathState == true)
            {
                playerGameObject.SetActive(false);
                GameObject deathPlayer = (GameObject)Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
                deathPlayer.transform.localScale = new Vector3(playerGameObject.transform.localScale.x, playerGameObject.transform.localScale.y, playerGameObject.transform.localScale.z);
                player.deathState = false;
                Invoke("ReloadLevel", 3);
            }

            if (ShowDeathMessage == true) //Show death message
                DeathMessageText.enabled = true;
            else
                DeathMessageText.enabled = false;

            if (ShowTimer == true) //Show timer
                TimerUI.enabled = true;
            else
                TimerUI.enabled = false;

            // Challange Code
            ShowTimer = false;
            if (CurrentChallange == 1) //Dont earn more than $5
            {
                CurrentChallangeUI.text = "Objective: Collect $5 to get yourself something nice";
                if (coinsCounter > 4)
                {
                    player.deathState = true;
                    DeathMessage = "Your wallet become to heavy!";
                    DeathMessageText.text = DeathMessage;
                    ShowDeathMessage = true;
                    CurrentChallange = -1;
                }
            }
            else if (CurrentChallange == 2)
                CurrentChallangeUI.text = "Objective: Don't commit any home invasion";
            else if (CurrentChallange == 3) //Dont Jump
            {
                CurrentChallangeUI.text = "Objective: Do some jumping jacks";
                if (player.HasJumped == true)
                {
                    player.deathState = true;
                    DeathMessage = "You broke your legs!";
                    DeathMessageText.text = DeathMessage;
                    ShowDeathMessage = true;
                    CurrentChallange = -1;
                }
            }
            else if (CurrentChallange == 4)
                CurrentChallangeUI.text = "Objective: Stay on planet earth";
            else if (CurrentChallange == 5)
            {
                CurrentChallangeUI.text = "Objective: Take your time :)";
                if (totalSeconds > TimeLimit - 1)
                {
                    TimerUI.text = "00:00";
                    player.deathState = true;
                    DeathMessage = "Times Up!";
                    DeathMessageText.text = DeathMessage;
                    ShowDeathMessage = true;
                    CurrentChallange = -1;
                }
                else
                {
                    TimerUI.text = timeDisplay;
                    
                }
                ShowTimer = true;
            }
            else if (CurrentChallange == 10)
            {
                CurrentChallange = 0;
                Application.Quit();
            }
                

            else
            {
                CurrentChallangeUI.text = "";
            }
        }

        public void ReloadLevel()
        {
            Application.LoadLevel(Application.loadedLevel);
        }


        public void LoadDialog()
        {
            DialogPointer = 0;
            //string TextShown = "";
            //string TextToShow = TextBoxDialog[DialogPointer];
            TextBoxSpeaker.text = TextBoxSpeakers[DialogPointer];

            TextBoxText.text = TextBoxDialog[DialogPointer];
            //for (int i = 0; i < TextToShow.Length; i++)
            //{
            //    TextShown = TextShown + TextToShow[i];
            //    TextBoxText.text = TextShown;
            //}
        }

        public void advanceTextBox()
        {
            DialogPointer++;
            if (DialogPointer == TextBoxDialog.Length)
            {
                InCutscene = false;
                player.StartJumpCooldown();

                if(ActionAfterDialog != 0)
                {
                    if(ActionAfterDialog == 1) //used for boss fight
                    {
                        GameObject boss = GameObject.Find("Boss Head");
                        boss.GetComponent<BossAttacks>().DieFully();
                        musicPlayer.GetComponent<ReserveSongs>().ChangeToSong1();
                    }
                    if (ActionAfterDialog == 2) //switch to reserve song 1
                    {
                        musicPlayer.GetComponent<ReserveSongs>().ChangeToSong1();
                    }
                    if (ActionAfterDialog == 3) //switch to reserve song 2
                    {
                        musicPlayer.GetComponent<ReserveSongs>().ChangeToSong2();
                    }
                }
            }
            else
            {
                TextBoxSpeaker.text = TextBoxSpeakers[DialogPointer];
                TextBoxText.text = TextBoxDialog[DialogPointer];
            }

        }
        public void BossDied()
        {
            
        }
    }
}
