using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text winTimerText;
    public TMP_Text loseTimerText;


    private float timer; // Initial timer value in seconds
    private bool isTimerRunning = false; // Flag to check if the timer is running
    
    private const float DEFAULT_TIME = 10f;

    public event Action OnTimerEnd;

    private void Awake()
    {
        timer = DEFAULT_TIME;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime; // Decrease the timer by the time passed since the last frame

            // Update the slider value based on the timer progress
            var currentTime = Mathf.Ceil(timer); 
            timerText.text = $"Timer: {currentTime.ToString(CultureInfo.CurrentCulture)}s.";
            winTimerText.text = $"Timer: {currentTime.ToString(CultureInfo.CurrentCulture)}s.";
            loseTimerText.text = $"Timer: {currentTime.ToString(CultureInfo.CurrentCulture)}s.";


            if (timer <= 0f)
            {
                timer = 0f; // Ensure the timer does not go negative
                isTimerRunning = false; // Stop the timer when it reaches 0
                OnTimerEnd?.Invoke();
            }
        }
    }

    public void StartTimer()
    { 
        // Reset the timer to 5 seconds
        isTimerRunning = true; // Start the timer
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        timer = DEFAULT_TIME;
    }
}
