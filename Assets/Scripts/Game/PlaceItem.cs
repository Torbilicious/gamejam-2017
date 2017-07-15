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

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (hit.transform.tag.Equals("ClickableTile"))
            {
                Tile tile = hit.transform.parent.GetComponent<Tile>();
                if (tile.Placeable.transform.childCount != 0) return;
                Transform spawned = ModelStore.Instance.Get(selected);
                if (spawned == null) return;
                spawned = Instantiate(spawned, tile.Placeable.transform);

                Vector2 distanceFromTileCenter;
                distanceFromTileCenter = new Vector3(hit.point.x - hit.transform.position.x, hit.point.z - hit.transform.position.z);
                if (Mathf.Abs(distanceFromTileCenter.y) > Mathf.Abs(distanceFromTileCenter.x))
                {
                    if (distanceFromTileCenter.y > 0) spawned.transform.localEulerAngles = Vector3.up * 0;
                    else spawned.transform.localEulerAngles = Vector3.up * 180;
                }
                else
                {
                    if (distanceFromTileCenter.x > 0) spawned.transform.localEulerAngles = Vector3.up * 90;
                    else spawned.transform.localEulerAngles = Vector3.up * 270;
                }

                playerMouseMode = PlayerMouseMode.FREE;
            }
        }
    }
}