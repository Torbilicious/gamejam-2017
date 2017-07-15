using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public bool WallNorth, WallEast, WallSouth, WallWest;

    [SerializeField] private GameObject placeable;

    [SerializeField] [Range(0, 1)] private float onFire;

    private bool HasPlaceable
    {
        get
        {
            if (placeable == null)
            {
                return false;
            }
            return placeable.transform.childCount == 0;
        }
    }

    private void Start()
    {
        new RunDirectionHelper();
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

    public void ApplyWalls()
    {
        GameObject.Find(gameObject.name + "/Walls/north").gameObject.SetActive(WallNorth);
        GameObject.Find(gameObject.name + "/Walls/east").gameObject.SetActive(WallEast);
        GameObject.Find(gameObject.name + "/Walls/south").gameObject.SetActive(WallSouth);
        GameObject.Find(gameObject.name + "/Walls/west").gameObject.SetActive(WallWest);
    }

    public static Tile Parse(string serialized)
    {
        if (serialized.Length < 5) return null;
        Transform o = Instantiate(ModelStore.Instance.baseTile);
        o.name += (long) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        Tile tile = o.gameObject.AddComponent<Tile>();
        char[] ser = serialized.ToCharArray();
        tile.WallNorth = ser[0] == '1';
        tile.WallEast = ser[1] == '1';
        tile.WallSouth = ser[2] == '1';
        tile.WallWest = ser[3] == '1';
        tile.ApplyWalls();
        return tile;
    }

    public Dictionary<RunDirection, bool> QueryDirections()
    {
        var directions = new Dictionary<RunDirection, bool>
        {
            {RunDirection.North, !WallNorth},
            {RunDirection.East, !WallEast},
            {RunDirection.South, !WallSouth},
            {RunDirection.West, !WallWest}
        };
        
        if (HasPlaceable)
        {
            var blockedDirection = RunDirectionHelper.ToDirection(transform.GetChild(0).localRotation.y);
            directions[blockedDirection] = false;
        }

        return directions;
    }
}