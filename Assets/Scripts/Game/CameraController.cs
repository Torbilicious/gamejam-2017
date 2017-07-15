using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float Speed = 10, ZoomSpeed = 10, MinFov = 40, MaxFov = 90;

    [SerializeField] private bool useMouse = true;

    [SerializeField] private Camera Cam;

    private Quaternion _initalCamQuaternion;
    private Vector3 _initalCam;
    
    
    private Vector3 _rortateOrigin;
    public float RotateSpeed = 30.0f;

    public float DragSpeed = 0.5f;
    private Vector3 _dragOrigin;

    // Use this for initialization
    private void Start()
    {
        mouse = Input.mousePosition;
        _initalCamQuaternion = Cam.transform.rotation;
        _initalCam = Cam.transform.localPosition;
    }

    private Vector2 mouse;

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
        if (Cam != null)
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
            pos.x * Time.deltaTime * DragSpeed,
            0, pos.y * Time.deltaTime * DragSpeed
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
            Mathf.Clamp((mouse.x - _rortateOrigin.x) * -1, -1, 1) * Time.deltaTime * RotateSpeed
        );
        mouse = Input.mousePosition;
    }

    private void ResetCamera()
    {
        transform.position = new Vector3(0, transform.position.y, 0);
        Cam.transform.localPosition = _initalCam;
        Cam.transform.rotation = _initalCamQuaternion;
    }

    private void ZoomCamera()
    {
//        Cam.fieldOfView += -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;

        Cam.transform.Translate(
            Vector3.forward * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed
        );


        Cam.fieldOfView = Mathf.Clamp(Cam.fieldOfView, MinFov, MaxFov);
    }

    private bool IsMouseOnScreen()
    {
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        return screenRect.Contains(Input.mousePosition);
    }
}