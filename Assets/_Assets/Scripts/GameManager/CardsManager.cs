using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    public GameObject deckUI;
    RectTransform rect;
    bool deckToggledOn = false;

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
        LeanTween.moveY(rect, -268f, .65f).setEase(LeanTweenType.easeInOutCubic); ;
    }
    public void ToggleDeckOn()
    {
        deckToggledOn = true;
        LeanTween.moveY(rect, -66, .65f).setEase(LeanTweenType.easeInOutCubic); ;
    }

}
