using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class StringSliter : MonoBehaviour
{
    [ContextMenu("CCAC")]
    private void FFF()
    {
        string[] file = File.ReadAllLines(Application.streamingAssetsPath + "/" + "Names.txt", Encoding.UTF8);
        for (var i = 0; i < file.Length; i++)
        {
            if(file[i] == "")
                print("isEmptey");
            file[i] = file[i].Split()[0];
        }
        File.WriteAllLines(Application.streamingAssetsPath + "/" + "Names1.txt", file);
    }
}
