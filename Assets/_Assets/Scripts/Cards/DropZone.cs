using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;


        print("eventData.pointerDrag" + eventData.pointerDrag);
        eventData.pointerDrag.GetComponent<Draggable>().placeholderParent = this.transform;
        // eventData.pointerDrag.GetComponent<Draggable>().placeholder.transform.SetParent(eventData.pointerDrag);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d.placeholderParent == this.transform) d.placeholderParent = d.parentToReturnTo;

    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        eventData.pointerDrag.GetComponent<Draggable>().parentToReturnTo = this.transform;
    }
}
