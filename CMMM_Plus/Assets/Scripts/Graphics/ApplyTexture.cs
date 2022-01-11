using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyTexture : MonoBehaviour
{
    void Start()
    {
        if(GetComponent<SpriteRenderer>() != null)
        {
            if (TextureLoader.textures.ContainsKey(GetComponent<SpriteRenderer>().sprite.name))
            GetComponent<SpriteRenderer>().sprite = TextureLoader.textures[GetComponent<SpriteRenderer>().sprite.name];
        }
        if (GetComponent<Image>() != null)
        {
            if (TextureLoader.textures.ContainsKey(GetComponent<Image>().sprite.name))
                GetComponent<Image>().sprite = TextureLoader.textures[GetComponent<Image>().sprite.name];
        }
    }
}
