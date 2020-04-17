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
    public Button craftBtn;
    public GameObject nextRoundAlert;
    public GameObject noTargetsAlert;
    public GameObject lootedAlert;
    public GameObject reshuffleAlert;

    bool displayText = false;

    public Quaternion healthBarsRotation;
    bool craftMenuOpened = false;
    public GameObject craftMenu;

    public GameObject deckHolder;
    Transform cardToAddToDeck;

    public GameObject cardDropZone;
    DropZone dropZone;

    public TextMeshProUGUI discardPileUI;
    public TextMeshProUGUI lostPileUI;
    int discardPile = 0;
    int lostPile = 0;

    void Awake()
    {
        playerAnimator = player.GetComponentInChildren<Animator>();

        LeanTween.moveLocal(nextRoundAlert, new Vector3(0f, 1000f, 0), 0f);
        LeanTween.moveLocal(noTargetsAlert, new Vector3(0f, 1000f, 0), 0f);
        LeanTween.moveLocal(lootedAlert, new Vector3(0f, 1000f, 0), 0f);
        LeanTween.moveLocal(reshuffleAlert, new Vector3(0f, 1000f, 0), 0f);
        dropZone = cardDropZone.GetComponent<DropZone>();
    }
    void Start()
    {
        ChangeCardsPlayedOnTheUI();
    }

    void Update()
    {

    }
    public void ChangeRoundOnTheUI()
    {
        string newRound = roundManager.currentRound.ToString();
        displayRounds.text = newRound;
        DisplayNewRoundMessage("Begin Round " + newRound);

    }
    public void ChangeCardsPlayedOnTheUI()
    {
        cardsPlayed.text = cardsManager.cardsPlayed.ToString() + "/" + cardsManager.maxCardsToBePlayed.ToString() + " Cards Played";
    }

    public void EndTurn()
    {
        DisableEndTurn();
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
    public void DisableCrafting()
    {
        craftBtn.interactable = false;
    }
    public void EnableCrafting()
    {
        craftBtn.interactable = true;
    }



    public void DisplayNewRoundMessage(string message)
    {
        nextRoundAlert.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        StartCoroutine(AlertMessageCooldown(nextRoundAlert, new Vector3(0, 130, 0)));
    }
    public void DisplayNoTargetsMessage(string message)
    {
        noTargetsAlert.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        StartCoroutine(AlertMessageCooldown(noTargetsAlert, new Vector3(0, 50, 0)));
    }
    public void DisplayLootedMessage(string message)
    {
        lootedAlert.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        StartCoroutine(AlertMessageCooldown(lootedAlert, new Vector3(0, -30, 0)));
    }
    public void DisplayReshuffleMessage(string message)
    {
        reshuffleAlert.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        StartCoroutine(AlertMessageCooldown(reshuffleAlert, new Vector3(0, -110, 0)));
    }
    private IEnumerator AlertMessageCooldown(GameObject container, Vector3 position)
    {
        LeanTween.moveLocal(container, position, .5f);
        yield return new WaitForSeconds(3f);
        LeanTween.moveLocal(container, new Vector3(0f, 1000f, 0f), 2f);
    }



    public void ToggleCraftMenu()
    {
        if (craftMenuOpened)
        {
            craftMenuOpened = false;
            craftMenu.SetActive(false);
        }
        else
        {
            craftMenuOpened = true;
            craftMenu.SetActive(true);
        }
    }
    public void CraftCard()
    {
        if (cardDropZone.transform.GetChild(0).childCount > 0)
        {
            dropZone.CardDropZoneIsAvailable();
            cardToAddToDeck = null;
            if (cardDropZone.transform.childCount == 1) cardToAddToDeck = cardDropZone.transform.GetChild(0);
            if (cardToAddToDeck == null) return;
            else
            {
                // check if all card slots have been filled


                // Remove the card from the craftcard spot and add it to the deck.
                foreach (Transform ability in cardToAddToDeck)
                {
                    ability.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
                cardToAddToDeck.GetComponent<CardAbilities>().PutAbilitiesOnCard();
                cardToAddToDeck.SetParent(deckHolder.transform);
            }
        }
    }
    public void AddCardToDiscardPile()
    {
        discardPile++;
        discardPileUI.text = discardPile.ToString();
    }
    public void AddCardToLostPile()
    {
        lostPile++;
        lostPileUI.text = lostPile.ToString();
    }
}
