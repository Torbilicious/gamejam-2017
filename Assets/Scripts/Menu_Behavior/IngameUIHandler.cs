using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelectItem1()
    {
        PlaceItem.Instance.selected = "Placeable_Fire_Extinguisher";
    }

    public void OnSelectItem2()
    {
        PlaceItem.Instance.selected = "Placeable_Firealarm";
    }

    public void OnSelectItem3()
    {
        PlaceItem.Instance.selected = "Sprinkler";
    }

    public void OnSelectItem4()
    {
        PlaceItem.Instance.selected = "Placeable_Fire_Alarm_Button";
    }

    public void OnSelectItem5()
    {
        PlaceItem.Instance.selected = "Placeable_EXIT";
    }

    public void OnSelectItem6()
    {

    }

    public void OnSelectItem7()
    {

    }

    public void OnSelectItem8()
    {

    }

}
