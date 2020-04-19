using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float panSpeed = 8f;
    public float rotationSpeed = 10f;
    public float edgeSize = 30f;
    public bool sideScrollingIsActive = false;
    public bool cameraMovementEnabled = true;

    void Update()
    {

        Vector3 pos = this.transform.position;
        Quaternion rot = this.transform.rotation;
        if (
            cameraMovementEnabled &&
            Input.GetKey("w")
            || Input.GetKey("up")
            || (Input.mousePosition.y > Screen.height - edgeSize && sideScrollingIsActive)
        )
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (
            cameraMovementEnabled &&
            Input.GetKey("s")
            || Input.GetKey("down")
            || (Input.mousePosition.y < edgeSize && sideScrollingIsActive)
        )
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (
            cameraMovementEnabled &&
            Input.GetKey("a")
            || Input.GetKey("left")
            || (Input.mousePosition.x < edgeSize && sideScrollingIsActive)
        )
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (
            cameraMovementEnabled &&
            Input.GetKey("d")
            || Input.GetKey("right")
            || (Input.mousePosition.x > Screen.width - edgeSize && sideScrollingIsActive)
        )
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        this.transform.position = pos;

    }
}
