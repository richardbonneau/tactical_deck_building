using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        d.placeholderParent = this.transform;
        d.willPlayCard = false;
        if (eventData.pointerDrag.CompareTag("Ability") && this.gameObject.CompareTag("CraftCard"))
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
        d.willPlayCard = true;
        d.placeholderParent = null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        eventData.pointerDrag.GetComponent<Draggable>().parentToReturnTo = this.transform;
    }

}
