using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public AudioSource fadeIn;
    public AudioSource looping;

    static bool debounce;

    // Start is called before the first frame update
    void Start()
    {

        if (debounce) 
        {
            Destroy(gameObject);
            return;
        }
        debounce = true;
        DontDestroyOnLoad(gameObject);



        if (!PlayerPrefs.HasKey("Music Volume"))
        {
            PlayerPrefs.SetFloat("Music Volume", 1f);
        }

        if (!PlayerPrefs.HasKey("FX Volume"))
        {
            PlayerPrefs.SetFloat("FX Volume", 1f);
        }


        fadeIn.volume = PlayerPrefs.GetFloat("Music Volume");
        looping.volume = PlayerPrefs.GetFloat("Music Volume");

        fadeIn.Play();
        looping.PlayScheduled(AudioSettings.dspTime + fadeIn.clip.length);
        looping.loop = true;
    }

    public void volumeUpdate(float vol)
    {
        fadeIn.volume = vol;
        looping.volume = vol;
        PlayerPrefs.SetFloat("Music Volume", vol);
    }

}
