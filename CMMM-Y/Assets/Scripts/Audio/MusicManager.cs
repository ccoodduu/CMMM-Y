using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public AudioSource fadeIn;
    public AudioSource looping;

    public static MusicManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null) 
        {
            Destroy(gameObject);
            return;
        }
		instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize volumes
        if (!PlayerPrefs.HasKey("Music Volume"))
        {
            PlayerPrefs.SetFloat("Music Volume", 1f);
        }

        if (!PlayerPrefs.HasKey("FX Volume"))
        {
            PlayerPrefs.SetFloat("FX Volume", 1f);
        }

        UpdateVolume();

        // Start music
		fadeIn.Play();
        looping.PlayScheduled(AudioSettings.dspTime + fadeIn.clip.length);
        looping.loop = true;
    }

    public void UpdateVolume()
    {
        var vol = PlayerPrefs.GetFloat("Music Volume");

		fadeIn.volume = vol;
        looping.volume = vol;
    }

}
