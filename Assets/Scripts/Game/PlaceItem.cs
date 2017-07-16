using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItem : MonoBehaviour
{
    public void StartPlaceItem()
    {
        playerMouseMode = PlayerMouseMode.PLACING;
    }

    public static PlaceItem Instance;

    public enum PlayerMouseMode { PLACING, FREE }

    public PlayerMouseMode playerMouseMode = PlayerMouseMode.FREE;
    public string selected;

    // Use this for initialization
    private void Start()
    {
        Instance = this;
    }

    public GameObject itemButtons;
    private GameObject hovering = null;

    [SerializeField]
    private Texture2D tex_tile, tex_tile_north, tex_tile_east, tex_tile_south, tex_tile_west, tex_tile_not_allowed;

    // Update is called once per frame
    private void Update()
    {
        if (playerMouseMode == PlayerMouseMode.PLACING)
        {
            //Highlight where to place
            RaycastHit hoverHit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hoverHit);
            if (hoverHit.transform.tag.Equals("ClickableTile"))
            {
                if (hoverHit.transform.gameObject != hovering)
                {
                    if (hovering != null) hovering.GetComponent<Renderer>().material.mainTexture = tex_tile;
                    hovering = hoverHit.transform.gameObject;
                }
                if (hovering != null)
                {
                    Vector2 distanceFromTileCenter;
                    distanceFromTileCenter = new Vector3(hoverHit.point.x - hoverHit.transform.position.x, hoverHit.point.z - hoverHit.transform.position.z);
                    if (hovering.transform.parent.GetComponent<Tile>().Placeable.transform.childCount > 0)
                    {
                        hovering.GetComponent<Renderer>().material.mainTexture = tex_tile_not_allowed;
                    }
                    else if (Mathf.Abs(distanceFromTileCenter.y) > Mathf.Abs(distanceFromTileCenter.x))
                    {
                        if (distanceFromTileCenter.y > 0) hovering.GetComponent<Renderer>().material.mainTexture = tex_tile_north;
                        else hovering.GetComponent<Renderer>().material.mainTexture = tex_tile_south;
                    }
                    else
                    {
                        if (distanceFromTileCenter.x > 0) hovering.GetComponent<Renderer>().material.mainTexture = tex_tile_east;
                        else hovering.GetComponent<Renderer>().material.mainTexture = tex_tile_west;
                    }
                }
            }
            if (Input.GetMouseButtonDown(0) && playerMouseMode == PlayerMouseMode.PLACING)
            {
                new List<UnityEngine.UI.Button>(itemButtons.GetComponentsInChildren<UnityEngine.UI.Button>()).ForEach(b => b.interactable = true);
                playerMouseMode = PlayerMouseMode.FREE;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.tag.Equals("ClickableTile"))
                {
                    Debug.Log("Clickable was clicked!");
                    Tile tile = hit.transform.parent.GetComponent<Tile>();
                    if (tile.Placeable.transform.childCount != 0)
                    {
                        if (hovering != null) hovering.GetComponent<Renderer>().material.mainTexture = tex_tile;
                        return;
                    }
                    Transform spawned = ModelStore.Instance.Get(selected);
                    if (spawned == null)
                    {
                        if (hovering != null) hovering.GetComponent<Renderer>().material.mainTexture = tex_tile;
                        return;
                    }
                    spawned = Instantiate(spawned, tile.Placeable.transform);

                    Vector2 distanceFromTileCenter;
                    distanceFromTileCenter = new Vector3(hit.point.x - hit.transform.position.x, hit.point.z - hit.transform.position.z);
                    if (Mathf.Abs(distanceFromTileCenter.y) > Mathf.Abs(distanceFromTileCenter.x))
                    {
                        if (distanceFromTileCenter.y > 0)
                        {
                            tile.placeableDirection = RunDirection.North;
                            spawned.transform.localEulerAngles = Vector3.up * 0;
                        }
                        else
                        {
                            tile.placeableDirection = RunDirection.South;
                            spawned.transform.localEulerAngles = Vector3.up * 180;
                        }
                    }
                    else
                    {
                        if (distanceFromTileCenter.x > 0)
                        {
                            tile.placeableDirection = RunDirection.East;
                            spawned.transform.localEulerAngles = Vector3.up * 90;
                        }
                        else
                        {
                            tile.placeableDirection = RunDirection.West;
                            spawned.transform.localEulerAngles = Vector3.up * 270;
                        }
                    }
                    spawned.transform.localPosition = Vector3.zero;
                    playerMouseMode = PlayerMouseMode.FREE;
                    spawned.name = spawned.name.Replace("(Clone)", "");
                }
                if (hovering != null) hovering.GetComponent<Renderer>().material.mainTexture = tex_tile;
            }
        }
    }
}