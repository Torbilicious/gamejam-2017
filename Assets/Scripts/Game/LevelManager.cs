using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the main class for a level.
/// </summary>
public class LevelManager : MonoBehaviour {

    private bool _levelLoaded = false;

    public static string LevelName = "";

    public LevelModel LevelModel;

    /// <summary>
    /// Initialize the level
    /// </summary>
    void Start()
    {
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update ()
    {
		//Load the level
        if(!_levelLoaded && ModelStore.Instance != null)
        {
            _levelLoaded = true;
            TextAsset levelFile = Resources.Load("Levels/" + LevelName) as TextAsset;
            LevelModel.Deserialize(levelFile.text);
        }
	}
}
