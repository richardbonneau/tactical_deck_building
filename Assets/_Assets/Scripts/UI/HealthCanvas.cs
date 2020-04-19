using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCanvas : MonoBehaviour
{
    Quaternion originalRotation;
    Camera mainCam;
    UiManager uiManager;
    PlayerStatus playerStatus;
    EnemyStatus enemyStatus;
    Transform playerOrEnemy;
    RectTransform rect;

    void Awake()
    {
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
        rect = this.transform.GetChild(this.transform.childCount - 1).GetComponent<RectTransform>();


    }

    void Update()
    {
        if (enemyStatus == null)
        {
            rect.sizeDelta = new Vector2(playerStatus.health * 50 / playerStatus.maxHealth, 100);
        }
        else rect.sizeDelta = new Vector2(enemyStatus.health * 50 / enemyStatus.maxHealth, 100); ;
    }
    void LateUpdate()
    {
        this.transform.rotation = originalRotation;
    }
}