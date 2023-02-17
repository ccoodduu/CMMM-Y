using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMusicVolume : MonoBehaviour
{
    public void UpdateVolume(float vol)
    {
		PlayerPrefs.SetFloat("Music Volume", vol);
        MusicManager.instance.UpdateVolume();
    }
}
