using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public UiManager uiManager;
    public int currentRound = 1;

    void Start()
    {
        uiManager.ChangeRoundOnTheUI();
    }
    public void NextRound()
    {
        currentRound++;
        uiManager.ChangeRoundOnTheUI();
    }

}
