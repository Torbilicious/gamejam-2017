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
        get { return tiles; }
    }

    private const char SEPARATOR_X = ',', SEPARATOR_Z = '\n';

    public Tile TryGetTile(int x, int y)
    {
        try
        {
            return Tiles[x][y];
        }
        catch
        {
            return null;
        }
    }

    public String Serialize()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Tile[] row in tiles)
        {
            if (row != null)
            {
                foreach (Tile tile in row)
                {
                    if (tile != null) sb.Append(tile != null ? tile.ToString() : "");
                    if (sb.Length > 0) sb.Append(SEPARATOR_X);
                }
            }
            if (sb.Length > 0) sb.Append(SEPARATOR_Z);
        }
        string res = sb.ToString().Trim();
        return res.EndsWith(SEPARATOR_X + "") ? res.Substring(0, res.Length - 1) : res;
    }

    public void Deserialize(string serialized)
    {
        string[] rows = serialized.Split(SEPARATOR_Z);
        tiles = new Tile[rows.Length][];
        for (int r = 0; r < tiles.Length; r++)
        {
            string[] cols = rows[r].Split(SEPARATOR_X);
            tiles[r] = new Tile[cols.Length];
            for (int c = 0; c < cols.Length; c++)
            {
                tiles[r][c] = Tile.Parse(cols[c]);
                if (tiles[r][c] != null)
                {
                    tiles[r][c].gameObject.name += String.Format(" -- {0}:{1}", r, c);
                    Debug.Log(String.Format("{0}:{1} -- ", r, c) + cols[c]);
                    tiles[r][c].transform.position = new Vector3(c, 0, r);
                    tiles[r][c].indexY = r;
                    tiles[r][c].indexX = c;
                }
            }
        }
    }

    private void Start()
    {
        instance = this;
    }
}