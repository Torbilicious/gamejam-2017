using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LevelModel : MonoBehaviour
{
    private Tile[][] tiles;
    private const char SEPARATOR_X = ',', SEPARATOR_Y = '\n';

    public IEnumerator SerializeAsync(Action<String> onComplete)
    {
        StringBuilder sb = new StringBuilder();
        foreach (Tile[] row in tiles)
        {
            if (sb.Length > 0) sb.Append(SEPARATOR_Y);
            foreach (Tile tile in row)
            {
                if (sb.Length > 0) sb.Append(SEPARATOR_X);
                sb.Append(tile.ToString());
            }
        }
        yield return null;
        onComplete.Invoke(sb.ToString());
    }

    public IEnumerator DeserializeAsync(string serialized, Action<bool> onComplete)
    {
        bool error = false;
        string[] s = serialized.Split(SEPARATOR_Y);
        tiles = new Tile[s.Length][];
        for (int i = 0; i < tiles.Length; i++)
        {
            string[] row = s[i].Split(SEPARATOR_X);
            for (int j = 0; j < row.Length; j++)
            {
                tiles[i][j] = Tile.Parse(row[i]);
            }
        }
        yield return null;
        onComplete.Invoke(true);
    }
}