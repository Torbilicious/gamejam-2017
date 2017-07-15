﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public bool WallNorth, WallEast, WallSouth, WallWest;

    [SerializeField] private GameObject Placeable;

    [SerializeField] [Range(0, 1)] private float onFire;

    private bool HasPlaceable
    {
        get
        {
            if (Placeable == null)
            {
                return false;
            }
            return Placeable.transform.childCount != 0;
        }
    }

    public bool IsOnFire { get { return onFire > 0.1; } }

    private void Start()
    {
        new RunDirectionHelper();
        Placeable = GameObject.Find(gameObject.name + "/Placeable");
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(WallNorth ? '1' : '0');
        sb.Append(WallEast ? '1' : '0');
        sb.Append(WallSouth ? '1' : '0');
        sb.Append(WallWest ? '1' : '0');
        if (Placeable == null)
        {
            Placeable = GameObject.Find(gameObject.name + "/Placeable");
        }
        if (Placeable.transform.childCount != 0)
        {
            sb.Append(Placeable.transform.GetChild(0).name.Trim());
            switch ((int)Placeable.transform.GetChild(0).localEulerAngles.y)
            {
                case 0:
                    sb.Append('N');
                    break;

                case 90:
                    sb.Append('E');
                    break;

                case 180:
                    sb.Append('S');
                    break;

                case 270:
                    sb.Append('W');
                    break;
            }
        }

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
        if (serialized.Length < 4) return null;
        Transform o = Instantiate(ModelStore.Instance.baseTile);
        o.name += (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        Tile tile = o.GetComponent<Tile>();
        if (tile == null) tile = o.gameObject.AddComponent<Tile>();
        char[] ser = serialized.ToCharArray();
        tile.WallNorth = ser[0] == '1';
        tile.WallEast = ser[1] == '1';
        tile.WallSouth = ser[2] == '1';
        tile.WallWest = ser[3] == '1';
        char orientation = ser[ser.Length - 1];
        tile.ApplyWalls();
        if (serialized.Length == 4) return tile;
        serialized = serialized.Substring(4, ser.Length - 5);
        if (ser.Length > 0)
        {
            Transform t = ModelStore.Instance.Get(serialized);
            if (t != null)
            {
                t = Instantiate(t);
                tile.Placeable = GameObject.Find(tile.name + "/Placeable");
                t.SetParent(tile.Placeable.transform);
                t.localPosition = Vector3.zero;
                t.parent = tile.Placeable.transform;
                t.name = t.name.Replace("(Clone)", "").Trim();
                switch (orientation)
                {
                    case 'N':
                        t.localRotation = Quaternion.Euler(Vector3.up * 0);
                        break;

                    case 'E':
                        t.localRotation = Quaternion.Euler(Vector3.up * 90);
                        break;

                    case 'S':
                        t.localRotation = Quaternion.Euler(Vector3.up * 180);
                        break;

                    case 'W':
                        t.localRotation = Quaternion.Euler(Vector3.up * 270);
                        break;
                }
            }
        }
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

        try
        {
            if (HasPlaceable)
            {
                RunDirection direction;
                if (RunDirectionHelper.ToDirection(transform.GetChild(0).localRotation.y, out direction))
                {
                    directions[direction] = false;
                }
            }

            Tile north = LevelModel.Instance.TryGetTile((int)transform.position.z + 1, (int)transform.position.x);
            Tile south = LevelModel.Instance.TryGetTile((int)transform.position.z - 1, (int)transform.position.x);
            Tile east  = LevelModel.Instance.TryGetTile((int)transform.position.z, (int)transform.position.x + 1);
            Tile west  = LevelModel.Instance.TryGetTile((int)transform.position.z, (int)transform.position.x - 1);

            if (north && north.IsOnFire) directions[RunDirection.North] = false;
            if (south && south.IsOnFire) directions[RunDirection.South] = false;
            if (east  && east.IsOnFire)  directions[RunDirection.East]  = false;
            if (west  && west.IsOnFire)  directions[RunDirection.West]  = false;
        }
        catch {}
        return directions;

    }
}