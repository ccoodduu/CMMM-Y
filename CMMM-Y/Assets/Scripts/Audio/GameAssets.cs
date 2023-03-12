using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;
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
    }

    public AudioClip place;
    public AudioClip destroy;
}
