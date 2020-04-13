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
    [System.NonSerialized] public TextMeshProUGUI alertText;
    [System.NonSerialized] public RectTransform rect;
    public Quaternion healthBarsRotation;
    bool craftMenuOpened = false;
    public GameObject craftMenu;
    public GameObject deckHolder;
    Transform cardToAddToDeck;

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

    public void ToggleCraftMenu(){
        if(craftMenuOpened){
            craftMenuOpened = false;
            craftMenu.SetActive(false);
        } else {
            craftMenuOpened = true;
            craftMenu.SetActive(true);
            }
    }
    public void CraftCard(){
        cardToAddToDeck = null;
        foreach(Transform child in craftMenu.transform){
            if(child.name == "Card") cardToAddToDeck = child;
        }
        if(cardToAddToDeck == null) return;
        else{
                // check if all card slots have been filled


        // Remove the card from the craftcard spot and add it to the deck.
            foreach(Transform ability in cardToAddToDeck){
                ability.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            cardToAddToDeck.GetComponent<CardAbilities>().PutAbilitiesOnCard();
            cardToAddToDeck.SetParent(deckHolder.transform);
        }
    
        
    }

}
