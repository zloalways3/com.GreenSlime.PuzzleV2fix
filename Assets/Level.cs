using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{

    [SerializeField] private GameObject _number;

    private Button _levelButton;

    private bool _isLocked = true;

    public bool IsLocked
    {
        get => _isLocked;
        set => _isLocked = value;
    }
    
    
    public delegate void ButtonClickedEventHandler(int buttonIndex);
    
    public event ButtonClickedEventHandler ButtonClickedEvent;

    private void Start()
    {
        _levelButton = GetComponent<Button>();
        _levelButton.onClick.AddListener(OnButtonClick);
    }
    
    void OnButtonClick()
    {
        if (ButtonClickedEvent == null) return;
        var number = _number.GetComponent<TMP_Text>().text;
        var i = Int32.Parse(number);
        ButtonClickedEvent(i);
    }

    public void Jrwerkwkfksdf()
    {
        _isLocked = false;
        if (_levelButton == null)
        {
            _levelButton = GetComponent<Button>();
        }
        _levelButton.interactable = true;
    }

    public bool CheckNumber(int level)
    {
        var number = _number.GetComponent<TMP_Text>().text;
        var i = Int32.Parse(number) - 1;
        return level == i;
    }
}
