﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public static bool playSounds = true;
    AudioSource source;
    static bool debounce;


    void Start()
    {

        if (debounce)
        {
            return;
        }
        debounce = true;
        DontDestroyOnLoad(gameObject);

        instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip sound) {
        if (!playSounds)
            return;
        
        source.volume = PlayerPrefs.GetFloat("FX Volume");
        source.PlayOneShot(sound);
    }
}
