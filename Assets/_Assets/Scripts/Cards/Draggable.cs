using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [System.NonSerialized] public Transform parentToReturnTo;
    public GameObject placeholderPrefab;
    GameObject placeholder;

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("begin drag");
        placeholder = Instantiate(placeholderPrefab, this.transform.position, Quaternion.identity);
        placeholder.transform.SetParent(this.transform.parent);
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;

        int newSiblingIndex = parentToReturnTo.childCount;
        for (int i = 0; i < parentToReturnTo.childCount; i++)
        {
            if (this.transform.position.x < parentToReturnTo.GetChild(i).position.x)
            {
                newSiblingIndex = i;
                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex) newSiblingIndex--;
                break;
            }

        }
        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        Destroy(placeholder);
    }

}
