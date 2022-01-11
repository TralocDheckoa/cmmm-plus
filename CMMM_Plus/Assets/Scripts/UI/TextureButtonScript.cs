using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextureButtonScript : MonoBehaviour
{
    // Token: 0x060000C1 RID: 193 RVA: 0x00006714 File Offset: 0x00004914
    public void OnClick()
    {
        string texturePath = transform.GetChild(4).gameObject.GetComponent<Text>().text.Split('/').First();

        PlayerPrefs.SetString("Texture", texturePath);
        TextureLoader.LoadTextureSet(texturePath);

        transform.parent.GetChild(0).GetChild(0).gameObject.SetActive(false);
        transform.SetAsFirstSibling();
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
