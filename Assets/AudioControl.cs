using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{

    [SerializeField] private AudioSource _musicSource;

    [SerializeField] private AudioSource _soundSource;

    private bool _musicState = true;
    
    private bool _soundState = true;

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;


    private void Update()
    {
        _musicSource.volume = _musicSlider.value;
        _soundSource.volume = _soundSlider.value;
    }

    public void PlaySound(AudioClip clip)
    {
        _soundSource.clip = clip;
        _soundSource.Play();
    }
}
