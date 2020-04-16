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
    void Awake()
    {
        if (this.transform.CompareTag("DeckHolder")) isDeckHolder = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || isCardDropZone && eventData.pointerDrag.CompareTag("Ability") || eventData.pointerDrag.CompareTag("Card") && this.transform.parent.tag == "DeckHolder") return;
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        d.placeholderParent = this.transform;
        d.willPlayCard = false;
        
        if (eventData.pointerDrag.CompareTag("Ability") && this.gameObject.CompareTag("Card"))
        {
            d.isPlacingAbilityOnCard = true;
            d.placeholderParent = this.transform;
            d.parentToReturnTo = this.transform;
        } 

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || this.transform.CompareTag("Card") && eventData.pointerDrag.CompareTag("Card")) return;
        if(isCardDropZone && dropZoneHasCard && eventData.pointerDrag.CompareTag("Card")) CardDropZoneIsAvailable();
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        d.placeholderParent = null;
        if (isDeckHolder)
        {
            d.willPlayCard = true;
        }
        if(eventData.pointerDrag.CompareTag("Ability") && this.gameObject.CompareTag("Card")){
            d.isPlacingAbilityOnCard = false;
            d.placeholderParent = this.transform.parent;
            d.parentToReturnTo = this.transform.parent;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {

        if (eventData.pointerDrag == null) return;
        if (eventData.pointerDrag.CompareTag("Ability") && this.transform.CompareTag("CardDropZone")) return;
        if (eventData.pointerDrag.CompareTag("Card") && this.transform.CompareTag("Card")) return;
        if (eventData.pointerDrag.CompareTag("Card") && isCardDropZone)
        {
            if (dropZoneHasCard) return;
            else {
                putCardHere.SetActive(false);
                dropZoneHasCard = true;
                }
        }


        eventData.pointerDrag.GetComponent<Draggable>().parentToReturnTo = this.transform;
    }
    public void CardDropZoneIsAvailable(){
          putCardHere.SetActive(true);
            dropZoneHasCard = false;
    }

}



