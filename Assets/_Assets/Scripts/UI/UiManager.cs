using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public RoundManager roundManager;
    public Pathfinder pathfinder;
    public GameObject player;
    Animator playerAnimator;
    public TextMeshProUGUI displayRounds;

    void Awake()
    {
        playerAnimator = player.GetComponentInChildren<Animator>();
    }
    public void EnablePlayerMove()
    {
        if (pathfinder.playerIsAllowedToMove == false) pathfinder.playerIsAllowedToMove = true;
        else
        {
            pathfinder.removeMovementGrid();
            pathfinder.playerIsAllowedToMove = false;
        }
    }
    public void EnablePlayerAttack()
    {
        playerAnimator.SetTrigger("Attack");
    }
    public void ChangeRoundOnTheUI()
    {
        displayRounds.text = roundManager.currentRound.ToString();
    }
    public void NextRound()
    {
        roundManager.NextRound();
    }


}
