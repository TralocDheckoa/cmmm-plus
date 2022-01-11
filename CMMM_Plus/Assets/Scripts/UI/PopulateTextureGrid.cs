using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssemblyCSharp.Assets.Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

public class PopulateTextureGrid : MonoBehaviour
{
    private void Start()
    {
        if (!Directory.Exists(Application.dataPath + "/texturepacks"))
        {
            Directory.CreateDirectory(Application.dataPath + "/texturepacks");
        }
        if (!Directory.Exists(Application.dataPath + "/texturepacks/Default"))
        {
            Directory.CreateDirectory(Application.dataPath + "/texturepacks/Default");
        }

        int length = (Application.dataPath + "/texturepacks/").Length;
        foreach (string text in Directory.GetDirectories(Application.dataPath + "/texturepacks/", "*", SearchOption.TopDirectoryOnly))
        {
            GameObject textureCard = Object.Instantiate<GameObject>(prefab, gameObject.transform);

            GameObject textureCardBG = textureCard.transform.GetChild(0).gameObject;
            GameObject textureCardTitle = textureCard.transform.GetChild(1).gameObject;
            GameObject textureCardDesc = textureCard.transform.GetChild(2).gameObject;
            GameObject textureCardImg = textureCard.transform.GetChild(3).gameObject;
            GameObject textureCardPath = textureCard.transform.GetChild(4).gameObject;

            textureCardPath.GetComponent<Text>().text = text.Split('/').Last() + "/";

            string currentPack = PlayerPrefs.GetString("Texture", "Default");

            // try to load pack.json, default to folder name
            try
            {
                TexturePackData texturePackData = JsonUtility.FromJson<TexturePackData>(System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(text + "/pack.json")));

                if (texturePackData.title == "Default" && (text.Split('/').Last() != "Default")) { Destroy(textureCard); continue; }

                textureCardTitle.GetComponent<Text>().text = texturePackData.title;
                if (currentPack == text.Split('/').Last())
                {
                    textureCardBG.SetActive(true);
                    textureCard.transform.SetAsFirstSibling();
                }

                textureCardDesc.GetComponent<Text>().text = texturePackData.desc;
            }
            catch
            {
                textureCardTitle.GetComponent<Text>().text = text.Split(new char[] {
                    '/'
                })[text.Split(new char[] {
                    '/'
                }).Length - 1];

                if (currentPack == text.Split('/').Last())
                {
                    textureCardBG.SetActive(true);
                    textureCard.transform.SetAsFirstSibling();
                }

                textureCardDesc.GetComponent<Text>().text = "";
            }

            // try to load pack image, default to no image
            try
            {
                byte[] imgData = File.ReadAllBytes(text + "/icon.png");
                Texture2D imgTex = new Texture2D(100, 100);
                imgTex.LoadImage(imgData);
                imgTex.filterMode = 0;

                textureCardImg.GetComponent<RawImage>().texture = imgTex;
            }
            catch
            {
            }
        }
    }
    public GameObject prefab;
}