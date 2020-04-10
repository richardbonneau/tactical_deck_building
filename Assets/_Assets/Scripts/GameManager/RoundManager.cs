using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public UiManager uiManager;
    public int currentRound = 1;
    public bool playerPhaseDone = false;
    public bool enemiesPhaseDone = false;

    public void PlayerPhaseIsDone()
    {
        playerPhaseDone = true;
    }
    void Start()
    {

        print("start RoundManager");
        uiManager.ChangeRoundOnTheUI();
    }
    void Update()
    {
    }
    public void NextRound()
    {
        currentRound++;
        uiManager.ChangeRoundOnTheUI();
    }

}
