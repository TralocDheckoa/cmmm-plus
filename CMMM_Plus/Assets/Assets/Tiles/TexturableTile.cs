using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu]
public class TexturableTile : Tile 
{
    private void Awake()
    {
        if (!Application.isEditor)
        {
            this.sprite = TextureLoader.textures[this.sprite.name];
        }
    }
}
