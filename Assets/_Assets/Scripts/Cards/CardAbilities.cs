using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbilities : MonoBehaviour
{
    public GameObject activeCardLocation;

    GameObject player;
    Pathfinder pathfinder;
    CardsManager cardsManager;
    Attacks attacks;
    public bool isMoveCard = false;
    public List<CardAction> cardActions = new List<CardAction>();


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

        player = GameObject.FindWithTag("Player");
        activeCardLocation = GameObject.FindWithTag("ActiveCardLocation");

    }
    void Start()
    {
        if (isMoveCard)
        {
            cardActions.Add(new CardAction(_actionType: "move", _value: 2));
            cardActions.Add(new CardAction(_actionType: "move", _value: 4));
        }
        else cardActions.Add(new CardAction(_actionType: "attack", _value: 5));
    }
    void Update()
    {
        if (currentActionIndex < cardActions.Count)
        {
            if (cardActive && !currentlyDoingAnAction)
            {
                PlayAction();
            }
        }
        else
        {
            CardUsed();
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
    }
    void CardUsed()
    {
        cardsManager.ToggleDeckOn();
        Destroy(this.gameObject);
    }
    void PlayAction()
    {
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
