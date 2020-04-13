using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    bool cardActive = false;
    public int currentActionIndex = 0;
    public bool currentlyDoingAnAction = false;
    void Awake()
    {
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
        PutAbilitiesOnCard();
    }
    void Update()
    {
        if (cardActions.Count == 0 || currentActionIndex < cardActions.Count)
        {

            if (cardActive && !currentlyDoingAnAction)
            {
                print("play action");
                PlayAction();
            }
        }
        else
        {
            CardUsed();
        }
    }

    public void PutAbilitiesOnCard(){
        print("put abilities on card");
        foreach(Transform child in this.transform)
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
        print(this.transform.GetChild(currentActionIndex)+" "+currentActionIndex);
       
        currentlyDoingAnAction = true;
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
        currentActionIndex++;
        currentlyDoingAnAction = false;
    }

}
