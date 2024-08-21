using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContentControl : MonoBehaviour
{
    [SerializeField] private TMP_Text _questionNumberText;
    [SerializeField] private TMP_Text _questionText;

    [SerializeField] private ContentBlockScripable _script;

    [SerializeField] private GameObject _go;
    [SerializeField] private GameObject _go2;

    [SerializeField] private GameObject _scoreBoard;
    [SerializeField] private TMP_Text _scoreBoardCounter;
    
    [SerializeField] private GameObject _firstPanel;

    [SerializeField] private Image _quizImage;

    [SerializeField] private List<TMP_Text> _answers;
    [SerializeField] private List<GameObject> _answersGO;

    private GameObject _currentBlockAnswer;

    [SerializeField] private Sprite _defaultAnswerState;
    [SerializeField] private Sprite _choosedAnswerState;

    [SerializeField] private Button _nextButton;

    private List<QuizModel> _currentList;
    private int _currentId;

    private bool _secondBlockShowed;

    private int _currentAnswer;

    private int _rightAnswersCount;

    private const int TRUE_FALSE_QUESTION = 2;

    public void ClickNext() {
        if (_currentList[_currentId].rightAnswer == _currentAnswer)
        {
            _rightAnswersCount++;
            Debug.Log("Правильно");
        }
        
        if (_currentId == _currentList.Count - 1) {
            _currentId = 0;
            _scoreBoardCounter.text = $"{_rightAnswersCount}/{_currentList.Count}";
            _scoreBoard.SetActive(true);
            return;
        }

        var image = _currentBlockAnswer.gameObject.GetComponent<Image>();
        image.sprite = _defaultAnswerState;
        
        _currentId++;
        LoadBlock();
    }

    public void LoadList() {
        _currentList = _script.Items;
    }
    
    public void LoadBlock() {
        _questionText.text = _currentList[_currentId].questionText;

        if (_currentList[_currentId].answerText.Count == TRUE_FALSE_QUESTION)
        {
            for (var i = _answersGO.Count; i > TRUE_FALSE_QUESTION; i--)
            {
                _answersGO[i - 1].SetActive(false);
            }
        }

        for (int i = 0; i < _currentList[_currentId].answerText.Count; i++)
        {
            _answers[i].text = _currentList[_currentId].answerText[i];
            _answersGO[i].SetActive(true);
            
        }
        
        if (_nextButton.interactable)
        {
            _nextButton.interactable = false;
        }
    }

    public void Exit() {
        Application.Quit();
    }

    public void SetCurrentQuestionAnswer(int answer)
    {
        if (!_nextButton.interactable)
        {
            _nextButton.interactable = true;
        }
        if (_currentBlockAnswer != null)
        {
            var oldImage = _currentBlockAnswer.gameObject.GetComponent<Image>();
            oldImage.sprite = _defaultAnswerState; 
        }
        _currentBlockAnswer = _answersGO[answer];
        var image = _currentBlockAnswer.gameObject.GetComponent<Image>();
        image.sprite = _choosedAnswerState;
        _currentAnswer = answer;
    }

    public void MainMenuState()
    {
        _scoreBoard.SetActive(false);
        _go2.SetActive(true);
        _go.SetActive(false);
        _rightAnswersCount = 0;
        _currentId = 0;
        if (_currentBlockAnswer == null) return;
        var image = _currentBlockAnswer.gameObject.GetComponent<Image>();
        image.sprite = _defaultAnswerState;
    }
}
