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
    public Button craftBtn;
    public GameObject alertTextContainer;
    [System.NonSerialized] public TextMeshProUGUI alertText;
    [System.NonSerialized] public RectTransform rect;
    public Quaternion healthBarsRotation;
    bool craftMenuOpened = false;
    public GameObject craftMenu;
    
    public GameObject deckHolder;
    Transform cardToAddToDeck;

    public GameObject cardDropZone;
    DropZone dropZone;

    void Awake()
    {
        playerAnimator = player.GetComponentInChildren<Animator>();
        rect = alertTextContainer.transform.GetChild(0).GetComponent<RectTransform>();
        alertText = alertTextContainer.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        LeanTween.moveLocal(alertTextContainer, new Vector3(0f, 0f, 1100f), 0f);
        dropZone = cardDropZone.GetComponent<DropZone>();
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
        craftBtn.interactable = false;

    }
    public void EnableEndTurn()
    {
        endTurnBtn.interactable = true;
        craftBtn.interactable = true;
    }
    public void DisplayAlertMessage(string message)
    {
        alertText.text = message;
        LeanTween.moveLocal(alertTextContainer, new Vector3(0f, 0f, 0f), .5f);
        LeanTween.moveLocal(alertTextContainer, new Vector3(0f, 1000f, 0f), 2f).setDelay(5f);
    }

    public void ToggleCraftMenu()
    {
        if (craftMenuOpened)
        {
            craftMenuOpened = false;
            craftMenu.SetActive(false);
        }
        else
        {
            craftMenuOpened = true;
            craftMenu.SetActive(true);
        }
    }
    public void CraftCard()
    {
        if (cardDropZone.transform.GetChild(0).childCount > 0)
        {
            dropZone.CardDropZoneIsAvailable();
            cardToAddToDeck = null;
            if (cardDropZone.transform.childCount == 1) cardToAddToDeck = cardDropZone.transform.GetChild(0);
            if (cardToAddToDeck == null) return;
            else
            {
                // check if all card slots have been filled


                // Remove the card from the craftcard spot and add it to the deck.
                foreach (Transform ability in cardToAddToDeck)
                {
                    ability.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
                cardToAddToDeck.GetComponent<CardAbilities>().PutAbilitiesOnCard();
                cardToAddToDeck.SetParent(deckHolder.transform);
            }

        }

    }

}
