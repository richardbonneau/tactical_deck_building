using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    bool isDeckHolder = false;
    public bool isCardDropZone = false;
    [System.NonSerialized] public bool dropZoneHasCard = false;
    void Awake()
    {
        if (this.transform.CompareTag("DeckHolder")) isDeckHolder = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        print("pointer enter");
        if (eventData.pointerDrag == null) return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        d.placeholderParent = this.transform;
        d.willPlayCard = false;
        if (eventData.pointerDrag.CompareTag("Ability") && this.gameObject.CompareTag("Card"))
        {
            d.isPlacingAbilityOnCard = true;
            d.parentToReturnTo = this.transform;
        };
        // eventData.pointerDrag.GetComponent<Draggable>().placeholder.transform.SetParent(eventData.pointerDrag);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        d.placeholderParent = null;
        if (isDeckHolder)
        {
            d.willPlayCard = true;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        print("pareant to returt to: " + eventData.pointerDrag.tag + " " + this.transform.tag);
        if (eventData.pointerDrag == null) return;
        if (eventData.pointerDrag.CompareTag("Ability") && this.transform.CompareTag("CardDropZone")) return;
        if (eventData.pointerDrag.CompareTag("Card") && this.transform.CompareTag("Card")) return;
        if (eventData.pointerDrag.CompareTag("Card") && isCardDropZone)
        {
            if (dropZoneHasCard) return;
            else dropZoneHasCard = true;
        }
        eventData.pointerDrag.GetComponent<Draggable>().parentToReturnTo = this.transform;

    }

}



