using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private void Start()
    {
        Ueqeqdasdasd();
    }

    [SerializeField] private List<Level> _levels;
    private int _fdjsdjfsjf = 1;

    private int _lfdlgdlwe;

    private void Ueqeqdasdasd()
    {
        _fdjsdjfsjf = PlayerPrefs.GetInt("CurrentLevel");
        if (_fdjsdjfsjf <= 0)
        {
            _fdjsdjfsjf = 1;
        }
        
        for (int i = 0; i < _fdjsdjfsjf; i++)
        {
            _levels[i].Jrwerkwkfksdf();
            _levels[i].ButtonClickedEvent += Tewrkrwkerksdf;
        }

        foreach (var t in _levels)
        {
            t.ButtonClickedEvent += Tewrkrwkerksdf;
        }
    }

    private void OnDestroy()
    {
        foreach (var t in _levels)
        {
            t.ButtonClickedEvent -= Tewrkrwkerksdf;
        }
    }

    public void Ukfsdkfskdfdfd()
    {
        if (_fdjsdjfsjf == _lfdlgdlwe)
        {
            _levels[_fdjsdjfsjf].Jrwerkwkfksdf();
            _fdjsdjfsjf++;
        }
        PlayerPrefs.SetInt($"CurrentLevel", _fdjsdjfsjf);
    }

    public void Njgfsdjgjdfjgd()
    {
        PlayerPrefs.SetInt($"CurrentLevel", 0);
    }

    private void Tewrkrwkerksdf(int buttonValue)
    {
        _lfdlgdlwe = buttonValue;
    }
}
