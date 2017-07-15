using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelStore : MonoBehaviour
{
    [SerializeField]
    private static ModelStore instance;

    public static ModelStore Instance
    {
        get { return instance; }
    }

    // Use this for initialization
    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}