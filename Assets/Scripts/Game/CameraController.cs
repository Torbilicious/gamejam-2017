﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float Speed = 10, ZoomSpeed = 10, MinFov = 40, MaxFov = 90;

    [SerializeField]
    private bool useMouse = true;

    [SerializeField]
    private Camera Cam;

    // Use this for initialization
    private void Start()
    {
        mouse = Input.mousePosition;
    }

    private Vector2 mouse;

    // Update is called once per frame
    private void Update()
    {
        if (!useMouse)
        {
            transform.Translate(Time.deltaTime * Speed * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
            if (Input.GetKey("q")) { transform.Rotate(transform.InverseTransformDirection(Vector3.down), Speed / 2); }
            if (Input.GetKey("e")) { transform.Rotate(transform.InverseTransformDirection(Vector3.down), Speed / -2); }
        }
        else if (!Input.GetMouseButton(1))
        {
            mouse = Input.mousePosition;
            if (mouse.x < Screen.width * 0.05) transform.Translate(Time.deltaTime * Speed * Vector3.left);
            if (mouse.x > Screen.width * 0.95) transform.Translate(Time.deltaTime * Speed * Vector3.right);

            if (mouse.y < Screen.height * 0.05) transform.Translate(Time.deltaTime * Speed * Vector3.forward * -1);
            if (mouse.y > Screen.height * 0.95) transform.Translate(Time.deltaTime * Speed * Vector3.forward);
        }
        else
        {
            transform.Rotate(transform.InverseTransformDirection(
                Vector3.down), (mouse.x - Input.mousePosition.x) * -1);
            mouse = Input.mousePosition;
        }
        if (Input.GetKeyDown("r")) transform.position = new Vector3(0, transform.position.y, 0);
        if (Cam != null)
        {
            Cam.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
            Cam.fieldOfView = Mathf.Clamp(Cam.fieldOfView, MinFov, MaxFov);
        }
    }
}