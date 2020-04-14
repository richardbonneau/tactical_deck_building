using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float panSpeed = 8f;
    public float rotationSpeed = 10f;

    void Update()
    {
        Vector3 pos = this.transform.position;
        Quaternion rot = this.transform.rotation;
        if (Input.GetKey("w"))
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        this.transform.position = pos;

        // if (Input.GetKey("q"))
        // {
        //     rot.y -= rotationSpeed * Time.deltaTime;
        // }
        // if (Input.GetKey("e"))
        // {
        //     rot.y += rotationSpeed * Time.deltaTime;
        // }
        // this.transform.rotation = rot;
    }
}
