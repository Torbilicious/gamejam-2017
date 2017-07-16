using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingSkin : MonoBehaviour
{
    public Material[] lemmingSkins;

    // Use this for initialization
    private void Start()
    {
        Material lemmingMat = lemmingSkins[Random.Range(0, lemmingSkins.Length - 1)];
        for (int i = 0; i < 6; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material = lemmingMat;
        }
    }
}