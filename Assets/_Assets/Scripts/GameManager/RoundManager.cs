using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public EnemiesManager enemiesManager;
    public UiManager uiManager;
    public int currentRound = 1;
    public bool playerPhaseDone = false;
    public bool enemiesPhaseDone = false;
    public int maxCardsToBePlayed = 2;
    public int cardsPlayed = 0;

    void Start()
    {
        uiManager.ChangeRoundOnTheUI();
    }


    public void NextRound()
    {
        currentRound++;
        uiManager.ChangeRoundOnTheUI();
        playerPhaseDone = false;
        enemiesPhaseDone = false;
        enemiesManager.NextRound();
    }

}
