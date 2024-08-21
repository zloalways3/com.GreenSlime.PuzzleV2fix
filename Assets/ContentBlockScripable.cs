using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "ContentBlock", menuName = "ScriptableObject", order = 1)]
public class ContentBlockScripable : ScriptableObject
{
    [HideInInspector] [SerializeField] private List<QuizModel> _items;
    [SerializeField] private QuizModel currentQuiz;

    private int currentIndex;

    public List<QuizModel> Items => _items;

    private Dictionary<string, List<QuizModel>> _sortedItems;

    private void OnEnable()
    {
        _sortedItems = new Dictionary<string, List<QuizModel>>();
    }

    #region SoInit
    public void CreateItem()
    {
        if (_items == null)
        {
            _items = new List<QuizModel>();
        }

        var item = new QuizModel();
        
        _items.Add(item);
        //item.ID = _items.Count;
        currentQuiz = item;
        currentIndex = _items.Count - 1;
    }

    public void RemoveItem()
    {
        _items.Remove(currentQuiz);
        if (_items.Count > 0)
            currentQuiz = _items[0];
        else CreateItem();
        currentIndex = 0;
    }

    public void NextItem()
    {
        if (currentIndex + 1 < _items.Count)
        {
            currentIndex++;
            currentQuiz = _items[currentIndex];
        }
    }
    public void PrevItem()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            currentQuiz = _items[currentIndex];
        }
    }
    #endregion
}

public enum PlayerType {
    CASINO
}

[System.Serializable]
public class QuizModel {
    public int id;

    public int questionNumber; // Start count from 1
    [TextArea(3, 10)]
    public string questionText;
    public int rightAnswer; // from 0 to n - 1 answers
    [CanBeNull] public Sprite image;
    public List<string> answerText = new List<string>(3); // capacity = answers count
}
