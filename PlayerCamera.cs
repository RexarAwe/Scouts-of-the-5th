using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Camera myCamera;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float zoomSpeed;

    void Start()
    {
        myCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if(Input.GetKey("w"))
        {
            transform.Translate(Vector3.up * cameraSpeed);
        }

        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * cameraSpeed);
        }

        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.down * cameraSpeed);
        }

        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * cameraSpeed);
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            myCamera.orthographicSize -= zoomSpeed;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            myCamera.orthographicSize += zoomSpeed;
        }
    }
}
