using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Transform baseTile, wall;

    [SerializeField]
    public bool WallNorth, WallEast, WallSouth, WallWest;

    [SerializeField]
    private GameObject placeable;

    [SerializeField]
    [Range(0, 1)]
    private float onFire = 0;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(WallNorth ? '1' : '0');
        sb.Append(WallEast ? '1' : '0');
        sb.Append(WallSouth ? '1' : '0');
        sb.Append(WallWest ? '1' : '0');
        sb.Append("basic");
        return sb.ToString();
    }

    public static Tile Parse(string serialized)
    {
        Tile tile = new Tile();
        tile.WallNorth = serialized[0] == 1;
        tile.WallEast  = serialized[1] == 1;
        tile.WallSouth = serialized[2] == 1;
        tile.WallWest  = serialized[3] == 1;
        return tile;
    }
}