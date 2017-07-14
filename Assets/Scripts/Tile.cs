using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Transform baseTile, wall;

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

    public static Tile Parse(string serialized)
    {
        return null;
    }
}