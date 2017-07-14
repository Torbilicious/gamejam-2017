using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Transform wallNorth, wallEast, wallSouth, wallWest;

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
        return "Tile";
    }

    public static Tile Parse(string serialized)
    {
        return null;
    }
}