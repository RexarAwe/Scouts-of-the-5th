using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Camera myCamera;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float zoomSpeed;

    [SerializeField] private float maxX;
    [SerializeField] private float maxY;
    [SerializeField] private float minX;
    [SerializeField] private float minY;

    void Start()
    {
        myCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if(Input.GetKey("w") && transform.position.y <= maxY)
        {
            transform.Translate(Vector3.up * cameraSpeed);
        }

        if (Input.GetKey("d") && transform.position.x <= maxX)
        {
            transform.Translate(Vector3.right * cameraSpeed);
        }

        if (Input.GetKey("s") && transform.position.y >= minY)
        {
            transform.Translate(Vector3.down * cameraSpeed);
        }

        if (Input.GetKey("a") && transform.position.x >= minX)
        {
            transform.Translate(Vector3.left * cameraSpeed);
        }

        //if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        //{
        //    myCamera.orthographicSize -= zoomSpeed;
        //}

        //if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        //{
        //    myCamera.orthographicSize += zoomSpeed;
        //}
    }
}
