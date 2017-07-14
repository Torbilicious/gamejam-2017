using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelEditor : Editor
{
    [MenuItem("FireLemmings/Serialize Level")]
    public static void SerializeLevel()
    {
        if (LevelModel.Instance == null)
        {
            Debug.LogError("No instance of LevelModel found in this scene!");
            return;
        }
        //LevelModel.Instance.SerializeAsync((string s) => { File.WriteAllText(Application.dataPath, s); });
        Debug.Log(Application.dataPath);
    }
}