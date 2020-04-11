using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        print("on pointer enter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        print("on pointer exit");
    }
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<Draggable>().parentToReturnTo = this.transform;
    }
}
