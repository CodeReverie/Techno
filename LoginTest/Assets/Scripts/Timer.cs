using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 120;
    public bool timeIsRunning = true;
    public TMP_Text timeText;
    public TMP_Text PhaseText;
    public bool nightPhase = true;
    public bool gameIsRunning = true;

   
    void Start()
    {
        timeIsRunning = true;
        nightPhase = true;
        timeRemaining = 5; 
    }

    
    void Update()
    {
        if (timeIsRunning && nightPhase)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                SwitchPhase("Day Phase");
            }
        }
        else if (timeIsRunning && !nightPhase)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                SwitchPhase("Night Phase");
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void SwitchPhase(string newPhase)
    {
        timeIsRunning = true;
        timeRemaining = 0;
        DisplayTime(timeRemaining);
        PhaseText.text = newPhase;

        if (newPhase == "Day Phase")
        {
            timeRemaining = 5;
        }
        else if (newPhase == "Night Phase")
        {
            timeRemaining = 5;
        }

        nightPhase = !nightPhase; // Switch the phase
    }
}
