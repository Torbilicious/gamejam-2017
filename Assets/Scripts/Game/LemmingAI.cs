using System;
using UnityEngine;

public partial class LemmingAI : MonoBehaviour {

    #region Internal Structures

    private enum LemmingState
    {
        NotLaunched,
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

    private Tile _currentTile;
    private Tile _nextTile;

    /// <summary>
    /// The lemming is currently in this state.
    /// </summary>
    private LemmingState _state = LemmingState.NotLaunched;

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
        _currentTile = null;
        _nextTile = null;
        _lemmingScript = (Lemming)LemmingScript;

        //StartAI();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(_state == LemmingState.NotLaunched)
        {
            return;
        }

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

    public void StartAI()
    {
        _state = LemmingState.Idle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<LemmingAI>() != null)
            FindNextWaypoint(true);
    }

    private void QueryFireState()
    {
//        if (_currentTile.fire > 0)
//        {
//            _state = LemmingState.Burning;
//        }
    }

    private void FindNextWaypoint(bool ignoreLast = false)
    {
        if (CanRunTo(_lastRunDirection) && !ignoreLast)
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
    }

    private void RunToWaypoint()
    {
        Vector3 targetPos = _nextTile.transform.position + new Vector3(0, 1, 0);
        float step = _lemmingScript.Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        Vector3 relativePos = targetPos - this.gameObject.transform.position;
        if (relativePos != Vector3.zero)
        {
            //relativePos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            this.gameObject.transform.rotation = Quaternion.Slerp(rotation, this.gameObject.transform.rotation, 0.001f);
        }

        if (transform.position == targetPos)
        {
            _state = LemmingState.WaypointReached;
        }
    }

    private bool CanRunTo(RunDirection direction)
    {
        if(_currentTile == null)
        {
            return false;
        }
        var directions = _currentTile.QueryDirections();
        return directions[direction];
    }

    private void CheckForObjectInteraction()
    {
        //TODO: check here if there is a placeable on the current tile
        //set LemmingSTate.Idle if no interaction is possible

        if (_currentTile) ;

        _state = LemmingState.Idle;
    }

    private void SetNextWayPoint(RunDirection direction)
    {
        try
        {
            //Find the next tile
            Vector3 currentTilePos = _currentTile.gameObject.transform.position;

            switch (direction)
            {
                case RunDirection.North:
                    _nextTile = LevelModel.Instance.Tiles[(int)currentTilePos.z + 1][(int)currentTilePos.x];
                    break;
                case RunDirection.South:
                    _nextTile = LevelModel.Instance.Tiles[(int)currentTilePos.z - 1][(int)currentTilePos.x];
                    break;
                case RunDirection.East:
                    _nextTile = LevelModel.Instance.Tiles[(int)currentTilePos.z][(int)currentTilePos.x + 1];
                    break;
                case RunDirection.West:
                    _nextTile = LevelModel.Instance.Tiles[(int)currentTilePos.z][(int)currentTilePos.x - 1];
                    break;
            }
        }
        catch
        {
            LevelManager.Points++;
            Destroy(this.gameObject);
        }
    }
}