using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float panSpeed = 8f;
    public float rotateSpeed = 100f;
    public float rotationSpeed = 10f;
    public float edgeSize = 30f;
    public bool sideScrollingIsActive = false;
    public bool cameraMovementEnabled = true;

    bool currentlyRotating = false;
    string rotationDirection = "left";
    float newYValue = 0f;
    public float rotationAmount = 45;

    void Update()
    {
        Vector3 pos = this.transform.position;
        float rot = this.transform.localRotation.eulerAngles.y;
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

        if (Input.GetKeyDown("e") && !currentlyRotating)
        {
            currentlyRotating = true;
            rotationDirection = "left";
            newYValue = rot + rotationAmount;
            if (newYValue >= 360) newYValue = 0;
        }
        if (Input.GetKeyDown("q") && !currentlyRotating)
        {
            currentlyRotating = true;
            rotationDirection = "right";
            newYValue = rot - rotationAmount;
            if (newYValue >= 360) newYValue = 0;
        }
        this.transform.position = pos;


        if (currentlyRotating && rotationDirection == "left")
        {
            if (rot < newYValue || newYValue == 0 && rot > 0 && rot > 5)
            {

                this.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
            }
            else if (rot > newYValue)
            {
                Vector3 eulerAngles = this.transform.eulerAngles;
                eulerAngles.y = newYValue;
                this.transform.eulerAngles = eulerAngles;
                currentlyRotating = false;
            }
        }
        else if (currentlyRotating && rotationDirection == "right")
        {
            if (rot > newYValue || newYValue == 0 && rot > 0 && rot < 355)
            {

                this.transform.Rotate(Vector3.down * Time.deltaTime * rotateSpeed);
            }
            else if (rot < newYValue || (newYValue == 0 && rot > 355))
            {
                Vector3 eulerAngles = this.transform.eulerAngles;
                eulerAngles.y = newYValue;
                this.transform.eulerAngles = eulerAngles;
                currentlyRotating = false;
            }
        }

    }
}
