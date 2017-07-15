using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSource : MonoBehaviour
{
    [SerializeField]
    private float secondsBeforeIgnition = 10, randomAdditionalSeconds = 5;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void GameStarted()
    {
    }

    private IEnumerator IgniteDelayed()
    {
        yield return new WaitForSeconds(secondsBeforeIgnition + Random.Range(0, randomAdditionalSeconds));
        Tile tile = null;
        Transform parent = transform.parent;
        while (transform.parent != null && parent.GetComponent<Tile>() == null) parent = parent.parent;
        if (tile != null) tile.Ignite();
    }
}