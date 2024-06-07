using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeRemaining = 60f;
    public bool timerIsRunning = false;

    private void Start()
    {
      
    }
    public void Init()
    {
        // Start the timer automatically
        timerIsRunning = true;
    }
    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 1)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                GameManager.Instance.timerEnded = true;
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                GameManager.Instance.GameOver();
            }
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
