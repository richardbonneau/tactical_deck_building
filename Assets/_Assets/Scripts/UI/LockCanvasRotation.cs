using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCanvasRotation : MonoBehaviour
{
    Quaternion originalRotation;
    Camera mainCam;
    UiManager uiManager;

    void Awake()
    {
        mainCam = Camera.main;
        originalRotation = this.transform.rotation;
        uiManager = GameObject.FindWithTag("UiManager").GetComponent<UiManager>();
    }


    void LateUpdate()
    {
        if (this.gameObject.name == "PlayerCanvas")
        {
            this.transform.LookAt(mainCam.transform.position);
            uiManager.healthBarsRotation = this.transform.rotation;
        }

        else this.transform.rotation = uiManager.healthBarsRotation;

    }
}