using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow
{
    private const string LevelPath = "/Resources/Levels/";

    [MenuItem("Fire Lemmings/Serialize Level")]
    public static void SerializeLevel()
    {
        LevelEditor window = CreateInstance<LevelEditor>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 100);
        window.Show();
    }

    private string _filename = "level.csv";

    private void OnGUI()
    {
        GUILayout.Label("Level name");
        _filename = GUILayout.TextField(_filename);
        if (GUILayout.Button("Export"))
        {
            LevelModel lm = GameObject.FindObjectOfType<LevelModel>();
            if (lm == null) Debug.LogError("No instance of LevelModel found in this scene!");
            else
            {
                Debug.Log("Starting serialization of level!");
                string path = Application.dataPath + LevelPath;
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path += _filename;

                List<Tile> scannedTiles = new List<Tile>(FindObjectsOfType<Tile>());
                int maxX, maxZ;
                scannedTiles.Sort((Tile a, Tile b) =>
                {
                    return (int) (Math.Round(a.transform.position.z) - Math.Round(b.transform.position.z));
                });
                maxZ = (int) scannedTiles[scannedTiles.Count - 1].transform.position.z;
                scannedTiles.Sort((Tile a, Tile b) =>
                {
                    return (int) (Math.Round(a.transform.position.x) - Math.Round(b.transform.position.x));
                });
                maxX = (int) scannedTiles[scannedTiles.Count - 1].transform.position.x;
                Tile[][] sortedTiles = new Tile[maxZ + 1][];
                for (int i = 0; i < sortedTiles.Length; i++) sortedTiles[i] = new Tile[maxX + 1];
                foreach (Tile tile in scannedTiles)
                {
                    int z = (int) tile.transform.position.z;
                    int x = (int) tile.transform.position.x;
                    sortedTiles[z][x] = tile;
                }
                lm.Tiles = sortedTiles;
                string serialized = lm.Serialize();
                Debug.Log("Serialized level!");
                File.WriteAllText(path, serialized);
                Debug.Log("Generated level file!");
            }
        }
        if (GUILayout.Button("Import"))
        {
            ModelStore.Instance = GameObject.FindObjectOfType<ModelStore>();
            LevelModel lm = GameObject.FindObjectOfType<LevelModel>();
            if (lm == null) Debug.LogError("No instance of LevelModel found in this scene!");
            else
            {
                string level = File.ReadAllText(Application.dataPath + LevelPath + _filename);
                try
                {
                    foreach (Tile[] tiles in lm.Tiles)
                    foreach (Tile tile in tiles) if (tile != null) DestroyImmediate(tile.gameObject);
                }
                catch (Exception e)
                {
                }
                lm.Deserialize(level);
            }
        }
    }
}