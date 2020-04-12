using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public EnemiesManager enemiesManager;
    public RoundManager roundManager;
    public CardsManager cardsManager;
    public GameObject player;
    Animator playerAnimator;
    public TextMeshProUGUI displayRounds;

    void Awake()
    {
        playerAnimator = player.GetComponentInChildren<Animator>();
    }

    public void ChangeRoundOnTheUI()
    {
        displayRounds.text = roundManager.currentRound.ToString();
    }

    public void EndTurn()
    {
        roundManager.playerPhaseDone = true;
        enemiesManager.BeginEnemyPhase();
    }

    public void ToggleDeck()
    {
        cardsManager.ToggleDeck();
    }


}
