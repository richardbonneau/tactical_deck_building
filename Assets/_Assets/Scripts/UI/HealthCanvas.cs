using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCanvas : MonoBehaviour
{
    GameObject cameraHolder;
    CameraMovement cameraMovement;
    Quaternion originalRotation;
    Camera mainCam;
    UiManager uiManager;
    PlayerStatus playerStatus;
    EnemyStatus enemyStatus;
    Transform playerOrEnemy;
    RectTransform healthRect;
    RectTransform thisCanvas;


    void Awake()
    {
        cameraHolder = GameObject.FindWithTag("CameraHolder");
        cameraMovement = cameraHolder.GetComponent<CameraMovement>();
        playerOrEnemy = this.transform.parent;
        if (this.gameObject.name == "PlayerCanvas")
        {
            playerStatus = playerOrEnemy.GetComponent<PlayerStatus>();

        }
        else
        {
            enemyStatus = playerOrEnemy.GetComponent<EnemyStatus>();

        }
        originalRotation = this.transform.rotation;
        healthRect = this.transform.GetChild(this.transform.childCount - 1).GetComponent<RectTransform>();
        thisCanvas = this.GetComponent<RectTransform>();


    }

    void Update()
    {
        if (enemyStatus == null)
        {
            healthRect.sizeDelta = new Vector2(playerStatus.health * 50 / playerStatus.maxHealth, 100);
        }
        else healthRect.sizeDelta = new Vector2(enemyStatus.health * 50 / enemyStatus.maxHealth, 100); ;

        if (cameraMovement.cameraPosition == 0 && thisCanvas.transform.rotation.y != 0) thisCanvas.transform.rotation = Quaternion.Euler(45, 0, 0);
        else if (cameraMovement.cameraPosition == 1 && thisCanvas.transform.rotation.y != 90) thisCanvas.transform.rotation = Quaternion.Euler(45, 90, 0);
        else if (cameraMovement.cameraPosition == 2 && thisCanvas.transform.rotation.y != 180) thisCanvas.transform.rotation = Quaternion.Euler(45, 180, 0);
        else if (cameraMovement.cameraPosition == 3 && thisCanvas.transform.rotation.y != 270) thisCanvas.transform.rotation = Quaternion.Euler(45, 270, 0);
    }


}