using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    bool isDeckHolder = false;
    public bool isCardDropZone = false;
    [System.NonSerialized] public bool dropZoneHasCard = false;
    public GameObject putCardHere;
    CardAbilities cardAbilities;
    GameObject inventory;

    bool isCard = false;
    void Start()
    {
        if (this.gameObject.CompareTag("Card"))
        {
            isCard = true;
            cardAbilities = this.GetComponent<CardAbilities>();
            inventory = GameObject.FindWithTag("Inventory");
        }
        if (this.transform.CompareTag("DeckHolder")) isDeckHolder = true;
        if (this.transform.CompareTag("CardDropZone")) isCardDropZone = true;

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // print("HEREH EHRHERHERE   "+(isCard && eventData.pointerDrag.CompareTag("Ability") && this.transform.childCount >= cardAbilities.tier).ToString());
        if (eventData.pointerDrag == null || isCardDropZone && eventData.pointerDrag.CompareTag("Ability") || eventData.pointerDrag.CompareTag("Card") && this.transform.parent.tag == "DeckHolder") return;
        // if(isCard && eventData.pointerDrag.CompareTag("Ability") && this.transform.childCount >= cardAbilities.tier) return;
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        d.placeholderParent = this.transform;
        d.willPlayCard = false;

        if (eventData.pointerDrag.CompareTag("Ability") && isCard)
        {
            if (!cardAbilities.IsCardFull())
            {
                print(this.transform.childCount + " " + " " + cardAbilities.tier);
                d.isPlacingAbilityOnCard = true;
                d.placeholderParent = this.transform;
                d.parentToReturnTo = this.transform;
            }
            else d.stopDrag = true;

        }


    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || isCard && eventData.pointerDrag.CompareTag("Card")) return;
        if (isCardDropZone && dropZoneHasCard && eventData.pointerDrag.CompareTag("Card")) CardDropZoneIsAvailable();
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        d.placeholderParent = null;
        if (isDeckHolder)
        {
            d.willPlayCard = true;
        }
        if (eventData.pointerDrag.CompareTag("Ability") && isCard)
        {
            d.isPlacingAbilityOnCard = false;
            d.placeholderParent = this.transform.parent;
            d.parentToReturnTo = inventory.transform;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        print(this.gameObject.name + " " + this.gameObject.tag);
        if (eventData.pointerDrag == null) return;
        if (isCard && (eventData.pointerDrag.CompareTag("Card") || cardAbilities.IsCardFull()))
        {
            print("retrun 2");
            return;
        }

        if (isCardDropZone && eventData.pointerDrag.CompareTag("Ability")) return;

        if (isCardDropZone && eventData.pointerDrag.CompareTag("Card"))
        {
            if (dropZoneHasCard) return;
            else
            {
                putCardHere.SetActive(false);
                dropZoneHasCard = true;
            }
        }
        eventData.pointerDrag.GetComponent<Draggable>().parentToReturnTo = this.transform;
    }
    public void CardDropZoneIsAvailable()
    {
        putCardHere.SetActive(true);
        dropZoneHasCard = false;
    }

}



