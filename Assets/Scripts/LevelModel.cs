using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LevelModel : MonoBehaviour
{
    [SerializeField]
    private static LevelModel instance;

    public static LevelModel Instance
    {
        get { return instance; }
    }

    private Tile[][] tiles = { };

    public Tile[][] Tiles
    {
        set { tiles = value; }
    }

    private const char SEPARATOR_X = ',', SEPARATOR_Y = '\n';

    public String Serialize()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Tile[] row in tiles)
        {
            if (row == null) continue;
            if (sb.Length > 0) sb.Append(SEPARATOR_Y);
            foreach (Tile tile in row)
            {
                if (tile == null) continue;
                if (sb.Length > 0) sb.Append(SEPARATOR_X);
                sb.Append(tile.ToString());
            }
        }
        return sb.ToString();
    }

    public void DeserializeAsync(string serialized)
    {
        string[] rows = serialized.Split(SEPARATOR_Y);
        tiles = new Tile[rows.Length][];
        for (int row = 0; row < tiles.Length; row++)
        {
            string[] cols = rows[row].Split(SEPARATOR_X);
            tiles[row] = new Tile[cols.Length];
            for (int col = 0; col < cols.Length; col++)
            {
                tiles[row][col] = Tile.Parse(cols[col]);
                tiles[row][col].transform.position = new Vector3(col, 0, row);
            }
        }
    }

    private void Start()
    {
        instance = this;
    }
}