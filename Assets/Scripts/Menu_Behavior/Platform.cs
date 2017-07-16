using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject exitBtn;

    // Use this for initialization
    private void Start()
    {
#if WEBGL
        Destroy(exitBtn);
#endif
        Screen.fullScreen = true;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}