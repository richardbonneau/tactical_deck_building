using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardAbilities : MonoBehaviour
{
    GameObject activeCardLocation;

    Pathfinder pathfinder;
    UiManager uiManager;
    CardsManager cardsManager;
    Attacks attacks;
    List<CardAction> cardActions = new List<CardAction>();
    public List<string> actionsTypes = new List<string>();
    public List<int> actionsValues = new List<int>();
    public Image cardImage;
    public int tier = 1;
    


    bool cardActive = false;
    public int currentActionIndex = 0;
    public bool currentlyDoingAnAction = false;
    void Awake()
    {
        cardImage = this.GetComponent<Image>();
        GameObject gridManager = GameObject.FindWithTag("GridManager");
        pathfinder = gridManager.GetComponent<Pathfinder>();
        GameObject cardsManagerObj = GameObject.FindWithTag("CardsManager");
        cardsManager = cardsManagerObj.GetComponent<CardsManager>();
        attacks = cardsManagerObj.GetComponent<Attacks>();
        activeCardLocation = GameObject.FindWithTag("ActiveCardLocation");
        uiManager = GameObject.FindWithTag("UiManager").GetComponent<UiManager>();

    }
    void Start()
    {
        switch (tier)
        {
            case 1:
                cardImage.color = new Color32(70, 231, 0, 169);
                break;
            case 2:
                cardImage.color = new Color32(241, 83, 12, 169);
                break;
            case 3:
                cardImage.color = new Color32(231, 8, 222, 169);
                break;
            default:
                print("Card Tier not found");
                break;
        }
        PutAbilitiesOnCard();
    }
    void Update()
    {
        if (cardActions.Count == 0 || currentActionIndex < cardActions.Count)
        {
            if (cardActive && !currentlyDoingAnAction) PlayAction();
        }
        else CardUsed();

    }

    public void PutAbilitiesOnCard()
    {
        foreach (Transform child in this.transform)
        {
            child.GetComponent<SingleAction>().PutAbilityOnParentCard();
        }
        for (int i = 0; i < actionsValues.Count; i++)
        {
            string type = actionsTypes[i];
            int value = actionsValues[i];
            cardActions.Add(new CardAction(_actionType: type, _value: value));
        }
    }

    public bool IsCardFull(){
        print("is card full");
        print(tier+" "+this.transform.childCount);
        return this.transform.childCount >= tier;
    }
    public void PlayCard()
    {
        cardActive = true;
        pathfinder.originCard = this;
        attacks.originCard = this;
        cardsManager.ToggleDeckOff();
        LeanTween.move(this.gameObject, activeCardLocation.transform.position, 0.5f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.scale(this.gameObject.GetComponent<RectTransform>(), gameObject.GetComponent<RectTransform>().localScale * 1.5f, 0.5f);
        uiManager.DisableEndTurn();
    }
    void CardUsed()
    {
        Destroy(this.gameObject);
        cardsManager.CardUsed();
        uiManager.EnableEndTurn();
    }
    void PlayAction()
    {
        currentlyDoingAnAction = true;
        this.transform.GetChild(currentActionIndex).transform.GetChild(0).GetComponent<Image>().enabled = true;
        if (cardActions[currentActionIndex].actionType == "move") EnablePlayerMove(cardActions[currentActionIndex].value);
        else if (cardActions[currentActionIndex].actionType == "attack") attacks.FindPotentialTargets(cardActions[currentActionIndex].value);
    }
    public void EnablePlayerMove(int maxMove)
    {
        pathfinder.maxMove = maxMove;
        if (pathfinder.playerIsAllowedToMove == false) pathfinder.playerIsAllowedToMove = true;
        else
        {
            pathfinder.removeMovementGrid();
            pathfinder.playerIsAllowedToMove = false;
        }
    }
    public void NextAction()
    {
        this.transform.GetChild(currentActionIndex).transform.GetChild(0).GetComponent<Image>().enabled = false;
        currentActionIndex++;
        currentlyDoingAnAction = false;
    }

}
