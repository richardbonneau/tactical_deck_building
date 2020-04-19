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
    public Transform discardedCardsHolder;
    public GameObject deckHolder;


    void Awake()
    {
        rect = deckUI.GetComponent<RectTransform>();

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
        uiManager.ChangeCardsPlayedOnTheUI();
        ToggleDeckOn();

    }
    public void CardUsed(GameObject usedCard)
    {
        discardedCards.Add(usedCard);
        usedCard.transform.SetParent(discardedCardsHolder);
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

        uiManager.OpenLoseCardUI();
    }
    public void PutDiscardedCardsBackInMainDeck(GameObject lostCard)
    {
        foreach (GameObject card in discardedCards)
        {
            card.transform.SetParent(deckHolder.transform);
        }
        discardedCards.Clear();
        Destroy(lostCard);
        uiManager.AddCardToLostPile();
    }
    void PlayerTurnDone()
    {
        uiManager.EnableEndTurn();
    }

}
