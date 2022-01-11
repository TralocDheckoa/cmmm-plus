using System.IO;
using UnityEngine;

public class OpenTextureFolder : MonoBehaviour
{
    public void OpenFolderInExplorer()
    {
        if (!Directory.Exists(Application.dataPath + "/texturepacks"))
        {
            Directory.CreateDirectory(Application.dataPath + "/texturepacks");
        }
        Application.OpenURL("file://" + Application.dataPath + "/texturepacks");
    }
}
