using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;

    [SerializeField] private GameObject _losePanel;

    [SerializeField] private GameObject _levels;

    [SerializeField] private Timer _timer;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _winScoreText;
    [SerializeField] private TMP_Text _loseScoreText;


    private const int SCORE_INCREMENT = 5;

    private const int BASE_SCORE = 100;

    private int currentScore = 0;

    [SerializeField] private SwipeSelection _swipeSelection;
    [SerializeField] private LevelManager _level;
    


    // Start is called before the first frame update
    void OnEnable()
    {
        _timer.OnTimerEnd += TimerEnd;
    }
    
    private void OnDisable()
    {
        _timer.OnTimerEnd -= TimerEnd;
    }

    private void TimerEnd()
    {
        _losePanel.SetActive(true);
        _swipeSelection.SwitchBlockGame(true);
        _swipeSelection.PlayLose();
        _loseScoreText.text = $"Points: {currentScore}/{BASE_SCORE}";
    }

    public void MenuOpen()
    {
        _swipeSelection.SwitchBlockGame(true);
        _timer.StopTimer();
    }

    public void MenuClose()
    {
        _swipeSelection.SwitchBlockGame(false);
        _timer.StartTimer();
    }

    public bool ScoreAdd(int multiplayer)
    {
        currentScore += SCORE_INCREMENT * multiplayer;

        _scoreText.text = $"Points: {currentScore}/{BASE_SCORE}";

        if (currentScore < BASE_SCORE) return false;
        _scoreText.text = $"Points: {BASE_SCORE}" +
                          $"/{BASE_SCORE}";
            
        _winScoreText.text = $"Points: {BASE_SCORE}/{BASE_SCORE}";
        
        WinGame();
        return true;

    }

    public void WinGame()
    {
        _timer.StopTimer();
        _winPanel.SetActive(true);
        _swipeSelection.SwitchBlockGame(true);
        Debug.Log("WIN GAME");
        _winScoreText.text = $"Points: {currentScore}/{BASE_SCORE}";
        _level.Ukfsdkfskdfdfd();
    }

    public void ResetLevel()
    {
        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
        _timer.ResetTimer();
        _timer.StartTimer();
        currentScore = 0;
        _scoreText.text = $"Points: {currentScore}/{BASE_SCORE}";
        _swipeSelection.SwitchBlockGame(false);
        _swipeSelection.Restart();
    }

    public void ToLevels()
    {
        _levels.SetActive(true);
        _winPanel.SetActive(false);
        _swipeSelection.SwitchBlockGame(true);
        _timer.ResetTimer();
        _timer.StopTimer();
        currentScore = 0;
        _scoreText.text = $"Points: {currentScore}/{BASE_SCORE}";
        Debug.Log($"_swipeSelection.IsBlocked {_swipeSelection.IsBlocked}");
    }

    public void ToMainMenu()
    {
        _swipeSelection.SwitchBlockGame(false);
        _timer.ResetTimer();
        _timer.StopTimer();
        currentScore = 0;
        _scoreText.text = $"Points: {currentScore}/{BASE_SCORE}";
    }

    public void Exit()
    {
        Application.Quit();
    }
}
