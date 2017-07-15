using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    public bool WallNorth, WallEast, WallSouth, WallWest;

    [HideInInspector]
    [SerializeField]
    public GameObject Placeable;

    [SerializeField]
    [Range(0, 1)]
    public float onFire;

    private RunDirection placeableDirection;

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
        fire = GameObject.Find(gameObject.name + "/Fire");
        Placeable = GameObject.Find(gameObject.name + "/Placeable");
    }

    private GameObject fire;

    [SerializeField]
    private float burnTime = 4;

    private void Update()
    {
        if (fire != null) fire.SetActive(onFire > 0.05f);
        if (onFire > 0.05f && onFire < 1)
        {
            onFire += Time.deltaTime / burnTime;
        }
        if (onFire > 0.9f)
        {
            //Ignite other tiles
            if (!WallNorth && r != LevelModel.Instance.Tiles.Length - 1) { LevelModel.Instance.Tiles[r + 1][c].Ignite(); }
            if (!WallSouth && r != 0) { LevelModel.Instance.Tiles[r - 1][c].Ignite(); }
            if (!WallEast && c != LevelModel.Instance.Tiles[r].Length - 1) { LevelModel.Instance.Tiles[r][c + 1].Ignite(); }
            if (!WallWest && c != 0) { LevelModel.Instance.Tiles[r][c - 1].Ignite(); }
        }
    }

    [HideInInspector]
    public int c, r;

    internal void Ignite()
    {
        if (onFire < 0.05f)
        {
            onFire += 0.1f;
            Color color = transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color;
            transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = new Color(color.r * 0.2f, color.g * 0.1f, color.b * 0.1f);
        }
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
                        tile.placeableDirection = RunDirection.North;
                        t.localRotation = Quaternion.Euler(Vector3.up * 0);
                        break;

                    case 'E':
                        tile.placeableDirection = RunDirection.East;
                        t.localRotation = Quaternion.Euler(Vector3.up * 90);
                        break;

                    case 'S':
                        tile.placeableDirection = RunDirection.South;
                        t.localRotation = Quaternion.Euler(Vector3.up * 180);
                        break;

                    case 'W':
                        tile.placeableDirection = RunDirection.West;
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
                directions[placeableDirection] = false;
            }
        }
        catch { }
        return directions;
    }
}