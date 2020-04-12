using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UiManager : MonoBehaviour
{
    public EnemiesManager enemiesManager;
    public RoundManager roundManager;
    public CardsManager cardsManager;
    public GameObject player;
    Animator playerAnimator;
    public TextMeshProUGUI displayRounds;
    public TextMeshProUGUI cardsPlayed;
    public Button endTurnBtn;
    public GameObject alertTextContainer;
    public TextMeshProUGUI alertText;
    public RectTransform rect;

    void Awake()
    {
        playerAnimator = player.GetComponentInChildren<Animator>();
        rect = alertTextContainer.transform.GetChild(0).GetComponent<RectTransform>();
        alertText = alertTextContainer.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        LeanTween.moveLocal(alertTextContainer, new Vector3(0f, 0f, 1100f), 0f);

    }
    void Start()
    {
        ChangeCardsPlayedOnTheUI();
    }

    public void ChangeRoundOnTheUI()
    {
        string newRound = roundManager.currentRound.ToString();
        displayRounds.text = newRound;
        DisplayAlertMessage("Begin Round " + newRound);

    }
    public void ChangeCardsPlayedOnTheUI()
    {
        cardsPlayed.text = cardsManager.cardsPlayed.ToString() + "/" + cardsManager.maxCardsToBePlayed.ToString() + " Cards Played";
    }

    public void EndTurn()
    {
        roundManager.playerPhaseDone = true;
        enemiesManager.BeginEnemyPhase();
        cardsManager.ToggleDeckOff();
    }

    public void ToggleDeck()
    {
        cardsManager.ToggleDeck();
    }
    public void DisableEndTurn()
    {
        endTurnBtn.interactable = false;
    }
    public void EnableEndTurn()
    {
        endTurnBtn.interactable = true;
    }
    public void DisplayAlertMessage(string message)
    {
        alertText.text = message;
        LeanTween.moveLocal(alertTextContainer, new Vector3(0f, 0f, 0f), 1f);
        LeanTween.moveLocal(alertTextContainer, new Vector3(0f, 0f, 1100f), 1f).setDelay(3f);
    }


}
