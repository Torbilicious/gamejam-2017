﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _zoomSpeed = 10, _minFov = 40, _maxFov = 90;

    [SerializeField]
    private Camera _cam;

    private Quaternion _initalCamQuaternion;
    private Vector3 _initalCam;

    private Vector3 _rortateOrigin;
    public float RotateSpeed = 30.0f;

    public float DragSpeed = 0.5f;
    private Vector3 _dragOrigin;

    // Use this for initialization
    private void Start()
    {
        _mouse = Input.mousePosition;
        _initalCamQuaternion = _cam.transform.rotation;
        _initalCam = _cam.transform.localPosition;
    }

    private Vector2 _mouse;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(2) && IsMouseOnScreen())
        {
            MoveCamera();
        }
        else if (Input.GetMouseButton(1) && IsMouseOnScreen())
        {
            RotateCamera();
        }
        if (Input.GetKeyDown("r"))
        {
            ResetCamera();
        }
        if (_cam != null)
        {
            ZoomCamera();
        }
    }

    private void MoveCamera()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(2)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
        Vector3 move = new Vector3(
            pos.x * Time.deltaTime / Time.timeScale * DragSpeed,
            0, pos.y * Time.deltaTime / Time.timeScale * DragSpeed
        );

        transform.Translate(move, Space.Self);
    }

    private void RotateCamera()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _rortateOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) return;

        transform.Rotate(
            transform.InverseTransformDirection(Vector3.down),
            Mathf.Clamp((_mouse.x - _rortateOrigin.x) * -1, -1, 1) * Time.deltaTime / Time.timeScale * RotateSpeed
        );
        _mouse = Input.mousePosition;
    }

    private void ResetCamera()
    {
        transform.position = new Vector3(0, transform.position.y, 0);
        _cam.transform.localPosition = _initalCam;
        _cam.transform.rotation = _initalCamQuaternion;
    }

    private void ZoomCamera()
    {
        //        Cam.fieldOfView += -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;

        _cam.transform.Translate(
            Vector3.forward * Time.deltaTime / Time.timeScale * Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed
        );

        _cam.fieldOfView = Mathf.Clamp(_cam.fieldOfView, _minFov, _maxFov);
    }

    private static bool IsMouseOnScreen()
    {
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        return screenRect.Contains(Input.mousePosition);
    }
}