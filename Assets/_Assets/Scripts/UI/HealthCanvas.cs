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
    Image healthBar;
    Transform playerOrEnemy;
    RectTransform rect;
    int startingHealth = 0;
    void Awake()
    {
        playerOrEnemy = this.transform.parent;
        if (this.gameObject.name == "PlayerCanvas")
        {
            playerStatus = playerOrEnemy.GetComponent<PlayerStatus>();
            startingHealth = playerStatus.health;
        }
        else
        {
            enemyStatus = playerOrEnemy.GetComponent<EnemyStatus>();
            startingHealth = enemyStatus.health;
        }
        // mainCam = Camera.main;
        originalRotation = this.transform.rotation;
        // uiManager = GameObject.FindWithTag("UiManager").GetComponent<UiManager>();

        rect = this.transform.GetChild(this.transform.childCount - 1).GetComponent<RectTransform>();

    }

    void Update()
    {
        if (enemyStatus == null)
        {
            rect.sizeDelta = new Vector2(playerStatus.health * 100 / startingHealth, 100);
        }
        else rect.sizeDelta = new Vector2(enemyStatus.health * 100 / startingHealth, 100); ;
    }
    void LateUpdate()
    {
        this.transform.rotation = originalRotation;
        //     if (this.gameObject.name == "PlayerCanvas")
        //     {
        //         this.transform.LookAt(mainCam.transform.position);
        //         uiManager.healthBarsRotation = this.transform.rotation;
        //     }
        //     else this.transform.rotation = uiManager.healthBarsRotation;

    }
}