using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets i;
    static bool debounce;


    void Start()
    {
        if (debounce)
        {
            return;
        }
        debounce = true;
        DontDestroyOnLoad(gameObject);
        i = this;
    }

    public AudioClip place;
    public AudioClip destroy;
}
