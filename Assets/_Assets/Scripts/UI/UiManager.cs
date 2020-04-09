using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public RoundManager roundManager;
    public Pathfinder pathfinder;
    public TextMeshProUGUI displayRounds;

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

    }
    public void ChangeRoundOnTheUI()
    {
        displayRounds.text = roundManager.currentRound.ToString();
    }


}
