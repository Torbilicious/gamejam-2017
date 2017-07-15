﻿using System;
using UnityEngine;

public class LemmingAI : MonoBehaviour {

    #region Internal Structures

    private enum RunDirection
    {
        North,
        South,
        West,
        East
    }

    private enum LemmingState
    {
        Idle,
        RunningToWaypoint,
        WaypointReached,
        Burning
    }

    #endregion

    #region Public Variables

    public GameObject GameManager;

    public MonoBehaviour LemmingScript;

    #endregion

    #region Private Variables

    /// <summary>
    /// The lemming is sourrounded by these tiles.
    /// |1|2|3|
    /// |-----|
    /// |4|5|6|
    /// |-----|
    /// |7|8|9|
    /// </summary>
    private Tile[] _sourroundingTiles;

    private Tile _currentTile;
    private Tile _nextTile;

    /// <summary>
    /// The lemming is currently in this state.
    /// </summary>
    private LemmingState _state = LemmingState.Idle;

    private RunDirection _lastRunDirection;

    private Lemming _lemmingScript;

    /// <summary>
    /// A reference to all tiles on the game level
    /// </summary>
    private static Tile[] _tiles;

    #endregion

    #region Public Methods

    public void OnWaypointReached()
    {
        _state = LemmingState.WaypointReached;
    }

    public void OnEnterTile()
    {
        //TODO: Get current tile here
    }

    #endregion

    // Use this for initialization
    void Start ()
    {
        _tiles = new Tile[0];
        _sourroundingTiles = new Tile[9];
        _currentTile = null;
        _nextTile = null;
        _lemmingScript = (Lemming)LemmingScript;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(_tiles.Length == 0)
        {
            _tiles = FindObjectsOfType<Tile>();
            return;
        }

        float shortestDistance = float.MaxValue;

        //Find the tile we are currently standing on
        foreach (Tile[] innerTiles in LevelModel.Instance.Tiles)
        {
            foreach (Tile tile in innerTiles)
            {
                if (tile != null)
                {
                    float distance = Vector3.Distance(tile.gameObject.transform.position, this.gameObject.transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        _currentTile = tile;
                    }
                }
            }
        }

	    QueryFireState();
        
        switch (_state)
        {
            case LemmingState.Burning:
                GetComponent<Animation>().Play("Lemming_Burning");
                break;
            case LemmingState.Idle:
                FindNextWaypoint();
                break;    
            case LemmingState.WaypointReached:
                CheckForObjectInteraction();
                break;
            case LemmingState.RunningToWaypoint:
                RunToWaypoint();
                break;
        }
	}

    private void QueryFireState()
    {
//        if (_currentTile.fire > 0)
//        {
//            _state = LemmingState.Burning;
//        }
    }

    private void FindNextWaypoint()
    {
        
        if (CanRunTo(_lastRunDirection))
        {
            SetNextWayPoint(_lastRunDirection);
        }
        else
        {
            RunDirection[] possibleDirection = (RunDirection[])Enum.GetValues(typeof(RunDirection));
            RunDirection nextDirection;

            do
            {
                nextDirection = (RunDirection)UnityEngine.Random.Range((int)possibleDirection[0], (int)possibleDirection[possibleDirection.Length - 1] + 1);
            }
            while (!CanRunTo(nextDirection));

            SetNextWayPoint(nextDirection);
            _lastRunDirection = nextDirection;
        }

        _state = LemmingState.RunningToWaypoint;

        //Find the next tile
        Vector3 currentTilePos = _currentTile.gameObject.transform.position;

        switch (_lastRunDirection)
        {
            case RunDirection.North:
                Debug.Log("AI: Running North");
                _nextTile = LevelModel.Instance.Tiles[(int)currentTilePos.z + 1][(int)currentTilePos.x];
                break;
            case RunDirection.South:
                Debug.Log("AI: Running South");
                _nextTile = LevelModel.Instance.Tiles[(int)currentTilePos.z - 1][(int)currentTilePos.x];
                break;
            case RunDirection.East:
                Debug.Log("AI: Running East");
                _nextTile = LevelModel.Instance.Tiles[(int)currentTilePos.z][(int)currentTilePos.x + 1];
                break;
            case RunDirection.West:
                Debug.Log("AI: Running West");
                _nextTile = LevelModel.Instance.Tiles[(int)currentTilePos.z][(int)currentTilePos.x - 1];
                break;
        }
    }

    private void RunToWaypoint()
    {
        Vector3 targetPos = _nextTile.transform.position + new Vector3(0, 1, 0);
        float step = _lemmingScript.Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        Vector3 relativePos = targetPos - this.gameObject.transform.position;
        if (relativePos != Vector3.zero)
        {
            relativePos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            this.gameObject.transform.rotation = Quaternion.Slerp(rotation, this.gameObject.transform.rotation, 0.001f);
        }

        if (transform.position == targetPos)
        {
            _state = LemmingState.WaypointReached;
        }
    }

    private void RefreshSourroundingTiles()
    {
        //TODO: set sourrounding tiles here
        //Add method in the game manager
    }

    private bool CanRunTo(RunDirection direction)
    {
        switch(direction)
        {
            case RunDirection.North:
                return !_currentTile.WallNorth;
            case RunDirection.South:
                return !_currentTile.WallSouth;
            case RunDirection.East:
                return !_currentTile.WallEast;
            case RunDirection.West:
                return !_currentTile.WallWest;
            default: return false;
        }
    }

    private void CheckForObjectInteraction()
    {
        //TODO: check here if there is a placeable on the current tile
        //set LemmingSTate.Idle if no interaction is possible

        _state = LemmingState.Idle;
    }

    private void SetNextWayPoint(RunDirection direction)
    {
        //TODO: set next waypoint here
    }
}