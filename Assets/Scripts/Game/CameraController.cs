using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float Speed = 10, ZoomSpeed = 10, MinFov = 40, MaxFov = 90;

    private Camera Cam;

    // Use this for initialization
    private void Start()
    {
        Cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Time.deltaTime * Speed * transform.InverseTransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))));
        if (Input.GetKeyDown("r")) transform.position = new Vector3(0, transform.position.y, 0);
        Cam.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        Cam.fieldOfView = Mathf.Clamp(Cam.fieldOfView, MinFov, MaxFov);
    }
}