using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    public GameObject deckUI;
    public UiManager uiManager;
    RectTransform rect;
    public GameObject player;
    bool deckToggledOn = true;
    public int maxCardsToBePlayed = 2;
    public int cardsPlayed = 0;
    public List<Sprite> abilityIcons = new List<Sprite>();
    public List<GameObject> discardedCards = new List<GameObject>();
    GameObject deckHolder;

    void Awake()
    {
        rect = deckUI.GetComponent<RectTransform>();
        deckHolder = GameObject.FindWithTag("DeckHolder");
    }

    public void ToggleDeck()
    {
        if (deckToggledOn) ToggleDeckOff();
        else ToggleDeckOn();
    }
    public void ToggleDeckOff()
    {
        deckToggledOn = false;
        LeanTween.moveY(rect, -398f, .65f).setEase(LeanTweenType.easeInOutCubic); ;
    }
    public void ToggleDeckOn()
    {
        deckToggledOn = true;
        LeanTween.moveY(rect, -66, .65f).setEase(LeanTweenType.easeInOutCubic);

    }
    public void NextRound()
    {
        cardsPlayed = 0;
        ToggleDeckOn();

    }
    public void CardUsed(GameObject usedCard)
    {
        discardedCards.Add(usedCard);
        usedCard.SetActive(false);
        uiManager.AddCardToDiscardPile();

        cardsPlayed++;
        uiManager.ChangeCardsPlayedOnTheUI();
        if (cardsPlayed < maxCardsToBePlayed)
        {
            ToggleDeckOn();
        }
        else
        {
            PlayerTurnDone();
        }

        if (deckHolder.transform.childCount < 1) ReShuffleAndLoseOne();

    }
    public void ReShuffleAndLoseOne()
    {
        GameObject cardToLose = discardedCards[Random.Range(0, discardedCards.Count)];
        discardedCards.Remove(cardToLose);

        foreach (GameObject card in discardedCards)
        {
            card.SetActive(true);
            card.transform.SetParent(deckHolder.transform);
        }
        // lose one card
        uiManager.DisplayReshuffleMessage("Deck Reshuffled! You lost a tier " + cardToLose.GetComponent<CardAbilities>().tier + " card in the process.");
        Destroy(cardToLose);
        uiManager.AddCardToLostPile();
    }
    void PlayerTurnDone()
    {
        uiManager.EnableEndTurn();
    }

}
