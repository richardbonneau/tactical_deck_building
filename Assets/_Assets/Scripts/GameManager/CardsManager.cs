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
        LeanTween.moveY(rect, -368f, .65f).setEase(LeanTweenType.easeInOutCubic); ;
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
    public void CardUsed()
    {
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

    }
    void PlayerTurnDone()
    {

    }

}
