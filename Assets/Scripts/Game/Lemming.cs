using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lemming : MonoBehaviour
{
    #region Internal Structures

    private enum LemmingState
    {
        NotLaunched,
        Idle,
        RunningToWaypoint,
        WaypointReached,
        Dead,
        UsingFireExtinguisher,
        UsingFireAlarmButton,
        UsingExitRight,
        UsingExitLeft
    }

    #endregion Internal Structures

    #region Public Variables

    public float Speed = 1.0f;

    #endregion Public Variables



    #region Private Variables

    [Range(0, 1)]
    [SerializeField]
    private float sanity = 1, courage = 1, health = 1;

    private Tile _currentTile;
    private Tile _nextTile;

    private float _removeTimer;
    private float _placeableUseTimer;

    /// <summary>
    /// The lemming is currently in this state.
    /// </summary>
    private LemmingState _state = LemmingState.NotLaunched;

    private RunDirection _lastRunDirection;

    /// <summary>
    /// A reference to all tiles on the game level
    /// </summary>
    private static Tile[] _tiles;

    #endregion Private Variables

    #region Public Methods

    public void StartAI()
    {
        _state = LemmingState.Idle;
        GetComponent<Animator>().Play("Lemming_Walking", -1, Random.Range(0.0f, 1.0f));
    }

   

    #endregion Public Methods

    // Use this for initialization
    private void Start()
    {
        _tiles = new Tile[0];
        _currentTile = null;
        _nextTile = null;
        _removeTimer = 3.0f;
    }

    private void Update()
    {
        if(LevelModel.Instance == null)
        {
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

        if (_state == LemmingState.NotLaunched)
        {
            return;
        }
        if (health <= 0)
        {
            _removeTimer -= Time.deltaTime;
            _state = LemmingState.Dead;
        }
        if (_removeTimer <= 0)
        {
            Destroy(this.gameObject);
        }

        if (_tiles.Length == 0)
        {
            _tiles = FindObjectsOfType<Tile>();
            return;
        }
        if (_currentTile.IsOnFire)
        {
            health -= Time.deltaTime / 100 * 70;
        }

        GetComponent<Animator>().SetBool("onFire", _currentTile.IsOnFire);
        GetComponent<Animator>().SetFloat("life", health);

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

            case LemmingState.UsingFireExtinguisher:
                HandleFireExtinguisher();
                break;

            case LemmingState.UsingFireAlarmButton:
                HandleFireAlarmButton();
                break;

            case LemmingState.UsingExitLeft:
                HandleExitSign(true);
                break;

            case LemmingState.UsingExitRight:
                HandleExitSign(false);
                break;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.StartsWith("Lemming"))
            FindNextWaypoint(true);
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
                int next;
                do
                {
                    next = UnityEngine.Random.Range((int)possibleDirection[0], (int)possibleDirection[possibleDirection.Length - 1] + 3);
                } while (next > (int)possibleDirection[possibleDirection.Length - 1]);
                nextDirection = (RunDirection)next;
            }
            while (!CanRunTo(nextDirection));

            SetNextWayPoint(nextDirection);
            _lastRunDirection = nextDirection;
        }

        _state = LemmingState.RunningToWaypoint;
    }

    private void RunToWaypoint()
    {
        Vector3 targetPos = Vector3.zero;
        try
        {
            targetPos = _nextTile.transform.position + new Vector3(0, 1, 0);
        }
        catch
        {
            LevelManager.Points++;
            Destroy(this.gameObject);
        }
        float step = this.Speed * Time.deltaTime;
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
        if (_currentTile == null)
        {
            return false;
        }
        return _currentTile.QueryDirections()[direction];
    }

    private void CheckForObjectInteraction()
    {
        Transform[] x = _currentTile.gameObject.GetComponentsInChildren<Transform>();
        if (_currentTile.Placeable.transform.childCount != 0)
        {
            Transform placeable = _currentTile.Placeable.transform.GetChild(0);

            switch (placeable.name)
            {
                case "Placeable_Fire_Extinguisher":
                    _placeableUseTimer = 3.0f;
                    _state = LemmingState.UsingFireExtinguisher;
                    Destroy(placeable.gameObject);
                    return;
                case "Placeable_Placeable_Fire_Alarm_Button":
                    _placeableUseTimer = 1.0f;
                    _state = LemmingState.UsingFireAlarmButton;
                    return;
                case "Placeable_EXIT_Right":
                    _placeableUseTimer = 0.5f;
                    _state = LemmingState.UsingExitRight;
                    return;
                case "Placeable_EXIT_Left":
                    _placeableUseTimer = 0.5f;
                    _state = LemmingState.UsingExitLeft;
                    return;
            }
        }

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

    #region Placeable Handling

    private void HandleFireExtinguisher()
    {
        for (int x = (int)_currentTile.transform.position.z - 1; x != (int)_currentTile.transform.position.z + 1; x++)
        {
            for (int y = (int)_currentTile.transform.position.x - 1; y != (int)_currentTile.transform.position.x + 1; y++)
            {
                Tile t = LevelModel.Instance.TryGetTile(x, y);
                if (t != null)
                {
                    t.onFire -= Time.deltaTime;
                    if (t.onFire < 0)
                    {
                        t.onFire = 0;
                    }
                }
            }
        }

        _placeableUseTimer -= Time.deltaTime;
        if(_placeableUseTimer <= 0)
        {
            _state = LemmingState.Idle;
        }
    }

    private void HandleFireAlarmButton()
    {
        LevelManager.FireAlarmTriggered = true;

        _placeableUseTimer -= Time.deltaTime;
        if (_placeableUseTimer <= 0)
        {
            _state = LemmingState.Idle;
        }

    }

    private void HandleExitSign(bool left)
    {
        _placeableUseTimer -= Time.deltaTime;
        if (_placeableUseTimer > 0)
        {
            return;
        }

        Transform placeable = _currentTile.Placeable.transform.GetChild(0);
        RunDirection direction;
        RunDirectionHelper.ToDirection(placeable.transform.rotation.y, out direction);
        if (left)
        {
            switch (direction)
            {
                case RunDirection.North:
                    direction = RunDirection.West;
                    break;
                case RunDirection.West:
                    direction = RunDirection.South;
                    break;
                case RunDirection.South:
                    direction = RunDirection.East;
                    break;
                case RunDirection.East:
                    direction = RunDirection.North;
                    break;
            } 
        }
        else
        {
            switch (direction)
            {
                case RunDirection.North:
                    direction = RunDirection.East;
                    break;
                case RunDirection.West:
                    direction = RunDirection.North;
                    break;
                case RunDirection.South:
                    direction = RunDirection.West;
                    break;
                case RunDirection.East:
                    direction = RunDirection.South;
                    break;
            }
        }

        if(CanRunTo(direction))
        {
            _lastRunDirection = direction;
            SetNextWayPoint(direction);
        }
        _state = LemmingState.RunningToWaypoint;
    }

    #endregion
}