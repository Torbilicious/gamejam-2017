using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lemming : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField]
    private float sanity = 1, courage = 1, health = 1;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}