using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbilities : MonoBehaviour
{
    public GameObject activeCardLocation;
    Animator playerAnimator;
    GameObject player;
    Pathfinder pathfinder;
    EnemiesManager enemiesManager;
    GridCreator gridCreator;
    CardsManager cardsManager;

    public bool isMoveCard = false;
    public List<CardAction> cardActions = new List<CardAction>();


    bool cardActive = false;
    public int currentActionIndex = 0;
    public bool currentlyDoingAnAction = false;
    void Awake()
    {
        GameObject gridManager = GameObject.FindWithTag("GridManager");
        pathfinder = gridManager.GetComponent<Pathfinder>();
        gridCreator = gridManager.GetComponent<GridCreator>();
        player = GameObject.FindWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
        enemiesManager = GameObject.FindWithTag("EnemiesManager").GetComponent<EnemiesManager>();
        activeCardLocation = GameObject.FindWithTag("ActiveCardLocation");
        cardsManager = GameObject.FindWithTag("CardsManager").GetComponent<CardsManager>();
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
            // done with this card
        }

    }

    public void PlayCard()
    {
        cardActive = true;
        pathfinder.originCard = this;
        cardsManager.ToggleDeckOff();
        LeanTween.move(this.gameObject, activeCardLocation.transform.position, 0.5f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.scale(this.gameObject.GetComponent<RectTransform>(), gameObject.GetComponent<RectTransform>().localScale * 1.5f, 0.5f);
    }
    void PlayAction()
    {
        currentlyDoingAnAction = true;
        if (cardActions[currentActionIndex].actionType == "move") EnablePlayerMove(cardActions[currentActionIndex].value);
        else if (cardActions[currentActionIndex].actionType == "attack") EnablePlayerAttack(cardActions[currentActionIndex].value);
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
    public void EnablePlayerAttack(int attackAmount)
    {
        List<GameObject> enemies = enemiesManager.activeEnemies;
        List<Node> neighboursNodes = gridCreator.GetNeighbours(gridCreator.NodeFromWorldPoint(player.transform.position));
        foreach (Node node in neighboursNodes)
        {
            GameObject found = enemies.Find(enemy => enemy.transform.position.x == node.worldPosition.x && enemy.transform.position.z == node.worldPosition.z);
            if (found != null)
            {
                playerAnimator.SetTrigger("Attack");
                int randomAnimation = Random.Range(1, 5);
                found.GetComponent<Animator>().SetTrigger("getHit" + randomAnimation);
                player.transform.LookAt(found.transform.position);

                found.GetComponent<EnemyStatus>().health = found.GetComponent<EnemyStatus>().health - 5;

                return;
            }
        }
    }
}
