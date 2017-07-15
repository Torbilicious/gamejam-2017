using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelStore : MonoBehaviour
{
    [SerializeField]
    public Transform baseTile;

    [SerializeField]
    public Transform baseLemming;

    [SerializeField]
    private Transform[] spawnables;

    [SerializeField]
    private static ModelStore instance;

    public static ModelStore Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    // Use this for initialization
    private void Start()
    {
        instance = this;
    }

    public Transform Get(string name)
    {
        foreach (Transform spawnable in spawnables)
        {
            if (spawnable != null && spawnable.name.Equals(name)) return spawnable;
        }
        return null;
    }
}