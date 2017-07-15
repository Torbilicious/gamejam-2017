using System;
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

    /// <summary>
    /// The lemming is currently in this state.
    /// </summary>
    private LemmingState _state;

    private RunDirection _lastRunDirection;

    private Lemming _lemmingScript;

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
        _sourroundingTiles = new Tile[9];
        _currentTile = null;
        _lemmingScript = (Lemming)LemmingScript;

    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (_state)
        {
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
                nextDirection = (RunDirection)UnityEngine.Random.Range((int)possibleDirection[0], (int)possibleDirection[possibleDirection.Length]);
            }
            while (!CanRunTo(nextDirection));

            SetNextWayPoint(nextDirection);
            _lastRunDirection = nextDirection;
            _state = LemmingState.RunningToWaypoint;
        }
    }

    private void RunToWaypoint()
    {
        float step = _lemmingScript.Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _currentTile.transform.position, step);
        transform.LookAt(_currentTile.transform);
    }

    private void RefreshSourroundingTiles()
    {
        //TODO: set sourrounding tiles here
        //Add method in the game manager
    }

    private bool CanRunTo(RunDirection direction)
    {
        return false;
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
    }

    private void SetNextWayPoint(RunDirection direction)
    {
        //TODO: set next waypoint here
    }
}