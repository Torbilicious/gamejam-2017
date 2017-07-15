using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private bool wallNorth, wallEast, wallSouth, wallWest;

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
        sb.Append(wallNorth ? '1' : '0');
        sb.Append(wallEast ? '1' : '0');
        sb.Append(wallSouth ? '1' : '0');
        sb.Append(wallWest ? '1' : '0');
        sb.Append("basic");
        return sb.ToString();
    }

    public void ApplyWalls()
    {
        GameObject.Find(gameObject.name + "/Walls/north").gameObject.SetActive(wallNorth);
        GameObject.Find(gameObject.name + "/Walls/east").gameObject.SetActive(wallEast);
        GameObject.Find(gameObject.name + "/Walls/south").gameObject.SetActive(wallSouth);
        GameObject.Find(gameObject.name + "/Walls/west").gameObject.SetActive(wallWest);
    }

    public static Tile Parse(string serialized)
    {
        if (serialized.Length < 5) return null;
        Transform o = Instantiate(ModelStore.Instance.baseTile);
        o.name += (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        Tile tile = o.gameObject.AddComponent<Tile>();
        char[] ser = serialized.ToCharArray();
        tile.wallNorth = ser[0] == '1';
        tile.wallEast = ser[1] == '1';
        tile.wallSouth = ser[2] == '1';
        tile.wallWest = ser[3] == '1';
        tile.ApplyWalls();
        return tile;
    }
}