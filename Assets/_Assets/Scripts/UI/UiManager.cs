using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UiManager : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public EnemiesManager enemiesManager;
    public RoundManager roundManager;
    public CardsManager cardsManager;
    public GameObject player;
    public GameObject mainCam;
    public bool centeringPlayer = false;
    Animator playerAnimator;
    public TextMeshProUGUI displayRounds;
    public TextMeshProUGUI cardsPlayed;
    public Button endTurnBtn;
    public Button craftBtn;
    public Button craftIt;
    public GameObject nextRoundAlert;
    public GameObject noTargetsAlert;
    public GameObject lootedAlert;
    public GameObject reshuffleAlert;
    public GameObject crafterAlert;

    bool displayText = false;

    public Quaternion healthBarsRotation;
    bool craftMenuOpened = false;
    public GameObject craftMenu;

    public GameObject deckHolder;
    Transform cardToAddToDeck;

    public GameObject cardDropZone;
    public GameObject loseCardDropZone;
    DropZone dropZone;
    DropZone loseCardDropZoneScript;

    public TextMeshProUGUI discardPileUI;
    public TextMeshProUGUI lostPileUI;
    int discardPile = 0;
    int lostPile = 0;

    public GameObject loseCardUI;
    public AudioSource audioSource;
    public AudioClip losecard;
    public AudioClip craft;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerAnimator = player.GetComponentInChildren<Animator>();
        LeanTween.moveLocal(crafterAlert, new Vector3(386f, 1000f, 0), 0f);
        LeanTween.moveLocal(nextRoundAlert, new Vector3(0f, 1000f, 0), 0f);
        LeanTween.moveLocal(noTargetsAlert, new Vector3(0f, 1000f, 0), 0f);
        LeanTween.moveLocal(lootedAlert, new Vector3(0f, 1000f, 0), 0f);
        LeanTween.moveLocal(reshuffleAlert, new Vector3(0f, 1000f, 0), 0f);
        dropZone = cardDropZone.GetComponent<DropZone>();
        loseCardDropZoneScript = loseCardDropZone.GetComponent<DropZone>();
    }
    void Start()
    {
        ChangeCardsPlayedOnTheUI();
    }

    void Update()
    {
        if (mainCam.transform.position == new Vector3(player.transform.position.x, mainCam.transform.position.y, player.transform.position.z - 6))
        {
            centeringPlayer = false;
            cameraMovement.cameraMovementEnabled = true;
        }
        if (centeringPlayer) mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, new Vector3(player.transform.position.x, mainCam.transform.position.y, player.transform.position.z - 6), 40 * Time.deltaTime);
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
        StartCoroutine(AlertMessageCooldown(nextRoundAlert, new Vector3(0, 130, 0), false));
    }
    public void DisplayNoTargetsMessage(string message)
    {
        noTargetsAlert.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        StartCoroutine(AlertMessageCooldown(noTargetsAlert, new Vector3(0, 50, 0), false));
    }
    public void DisplayLootedMessage(string message)
    {
        lootedAlert.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        StartCoroutine(AlertMessageCooldown(lootedAlert, new Vector3(0, -30, 0), false));
    }
    public void DisplayReshuffleMessage(string message)
    {
        reshuffleAlert.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        StartCoroutine(AlertMessageCooldown(reshuffleAlert, new Vector3(0, -110, 0), false));
    }
    public void DisplayCrafterMessage(string message)
    {
        crafterAlert.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        StartCoroutine(AlertMessageCooldown(crafterAlert, new Vector3(386f, 300f, 0f), true));
    }
    private IEnumerator AlertMessageCooldown(GameObject container, Vector3 position, bool isCrafter)
    {
        LeanTween.moveLocal(container, position, .5f);
        if (isCrafter) craftIt.interactable = false;
        yield return new WaitForSeconds(3f);
        float x = 0f;
        if (isCrafter)
        {
            x = 386f;
            craftIt.interactable = true;

        }
        LeanTween.moveLocal(container, new Vector3(x, 1000f, 0f), 1f);
    }

    public void ToggleCraftMenu()
    {
        if (craftMenuOpened)
        {
            craftMenuOpened = false;
            craftMenu.SetActive(false);
            cameraMovement.cameraMovementEnabled = true;
        }
        else
        {
            craftMenuOpened = true;
            craftMenu.SetActive(true);
            cameraMovement.cameraMovementEnabled = false;
        }
    }
    public void CraftCard()
    {
        audioSource.clip = craft;
        audioSource.Play();
        Transform card = cardDropZone.transform.GetChild(0);
        int tier = card.GetComponent<CardAbilities>().tier;

        if (card.childCount > 0 && tier == card.childCount)
        {
            dropZone.CardDropZoneIsAvailable();
            cardToAddToDeck = null;
            if (cardDropZone.transform.childCount == 1) cardToAddToDeck = cardDropZone.transform.GetChild(0);
            if (cardToAddToDeck == null) return;
            else
            {
                // Remove the card from the craftcard spot and add it to the deck.
                foreach (Transform ability in cardToAddToDeck)
                {
                    ability.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
                cardToAddToDeck.GetComponent<CardAbilities>().PutAbilitiesOnCard();
                cardToAddToDeck.SetParent(deckHolder.transform);
                DisplayCrafterMessage("Successfully Crafted a Card!");
            }
        }
        else
        {
            DisplayCrafterMessage("Your need to put " + tier + " abilities on this card in order to craft it");
        }
    }

    public void OpenLoseCardUI()
    {
        audioSource.clip = losecard;
        audioSource.Play();
        loseCardUI.SetActive(true);
        cameraMovement.cameraMovementEnabled = false;
    }
    public void CloseLoseCardUI()
    {
        loseCardUI.SetActive(false);
        cameraMovement.cameraMovementEnabled = true;
    }

    public void LoseCard()
    {
        GameObject card = loseCardDropZone.transform.GetChild(0).gameObject;

        if (card != null)
        {
            card.transform.SetParent(loseCardDropZone.transform.parent);
            cardsManager.PutDiscardedCardsBackInMainDeck(card);
            CloseLoseCardUI();
            loseCardDropZoneScript.CardDropZoneIsAvailable();
            lostPile++;
        }
    }

    public void AddCardToDiscardPile()
    {
        discardPile++;
        discardPileUI.text = discardPile.ToString();
    }
    public void AddCardToLostPile()
    {
        discardPile = 0;
        discardPileUI.text = discardPile.ToString();
        lostPile++;
        lostPileUI.text = lostPile.ToString();
    }


    public void CenterPlayer()
    {
        centeringPlayer = true;
        cameraMovement.cameraMovementEnabled = false;
    }
}
