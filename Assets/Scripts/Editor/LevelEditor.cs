using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow
{
    [MenuItem("Fire Lemmings/Serialize Level")]
    public static void SerializeLevel()
    {
        if (GameObject.FindObjectOfType<LevelModel>() == null)
        {
            Debug.LogError("No instance of LevelModel found in this scene!");
            return;
        }
        LevelEditor window = ScriptableObject.CreateInstance<LevelEditor>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 100);
        window.Show();
    }

    private void OnGUI()
    {
        string filename = GUILayout.TextField("level.csv");
        if (GUILayout.Button("Export"))
        {
            Debug.Log("Starting serialization of level!");
            string path = Application.dataPath + "/Levels/";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += filename;
            string serialized = GameObject.FindObjectOfType<LevelModel>().Serialize();
            Debug.Log("Serialized level!");
            File.WriteAllText(path, serialized);
            Debug.Log("Generated level file!");
        }
    }
}