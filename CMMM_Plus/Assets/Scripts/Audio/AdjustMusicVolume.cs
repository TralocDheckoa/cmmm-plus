using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMusicVolume : MonoBehaviour
{
    private GameObject Music;
    public void updateVolume(float vol)
    {
        Music = GameObject.FindGameObjectWithTag("Music");
        if (Music)
        {
            Music.GetComponent<MusicManager>().volumeUpdate(vol);
        }
    }
    
}
