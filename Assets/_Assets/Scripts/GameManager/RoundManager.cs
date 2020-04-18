using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public EnemiesManager enemiesManager;
    public UiManager uiManager;
    public CardsManager cardsManager;
    public int currentRound = 1;
    public bool playerPhaseDone = false;
    public bool enemiesPhaseDone = false;
    public PlayerStatus playerStatus;


    void Awake()
    {
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
    }
    void Start()
    {
        uiManager.ChangeRoundOnTheUI();

    }

    public void NextRound()
    {
        if (!playerStatus.isDead)
        {
            currentRound++;
            uiManager.ChangeRoundOnTheUI();
            playerPhaseDone = false;
            enemiesPhaseDone = false;
            enemiesManager.NextRound();
            cardsManager.NextRound();
        }
        else
        {
            uiManager.DisableEndTurn();
        }
    }
}
