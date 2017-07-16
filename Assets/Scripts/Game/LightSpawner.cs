using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpawner : MonoBehaviour
{
    public GameObject lightPrefab;

    // Use this for initialization
    private void Start()
    {
        new List<Light>(GameObject.FindObjectsOfType<Light>()).ForEach(s => { if (s.type == LightType.Directional) Destroy(s.gameObject); });
        Instantiate(lightPrefab);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}