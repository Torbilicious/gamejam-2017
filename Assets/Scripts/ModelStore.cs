using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelStore : MonoBehaviour
{
    [SerializeField]
    public struct Spawnable
    {
        public string key;
        public Transform transform;
    }

    [SerializeField]
    public Transform baseTile;

    [SerializeField]
    private Spawnable[] spawnables;

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

    public Transform Get(string key)
    {
        foreach (Spawnable spawnable in spawnables)
        {
            if (key.Equals(spawnable.key)) return spawnable.transform;
        }
        return null;
    }
}