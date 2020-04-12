using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    public GameObject deckUI;
    RectTransform rect;
    // Start is called before the first frame update
    void Awake()
    {
        rect = deckUI.GetComponent<RectTransform>();
    }

    // Update is called once per frame

    public void ToggleDeckOff()
    {
        LeanTween.moveY(rect, -268f, .65f).setEase(LeanTweenType.easeInOutCubic); ;
    }
    public void ToggleDeckOn()
    {
        LeanTween.moveY(rect, -66, .65f).setEase(LeanTweenType.easeInOutCubic); ;
    }

}
