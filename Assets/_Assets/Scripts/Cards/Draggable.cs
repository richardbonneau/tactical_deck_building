using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [System.NonSerialized] public Transform parentToReturnTo;
    [System.NonSerialized] public Transform placeholderParent;
    public GameObject placeholderPrefab;
    [System.NonSerialized] public GameObject placeholder;

    [System.NonSerialized] public bool willPlayCard = false;
    [System.NonSerialized] public bool isPlacingAbilityOnCard = false;
    bool isPlayableCard = false;
    bool isAbility = false;

    void Awake()
    {
        if (this.gameObject.name == "Card") isPlayableCard = true;
        else if (this.gameObject.name == "Ability") isAbility = true;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        placeholder = Instantiate(placeholderPrefab, this.transform.position, Quaternion.identity);
        placeholder.transform.SetParent(this.transform.parent);
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;

        if (placeholderParent == null) return;
        if (placeholder.transform.parent != placeholderParent) placeholder.transform.SetParent(placeholderParent);
        int newSiblingIndex = placeholderParent.childCount;
        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (isPlayableCard && this.transform.position.x < placeholderParent.GetChild(i).position.x || isAbility && this.transform.position.y > placeholderParent.GetChild(i).position.y)

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
        Destroy(placeholder);
        if (willPlayCard && isPlayableCard)
        {
            GetComponent<CardAbilities>().PlayCard();
            return;
        }
        else if (isPlacingAbilityOnCard && isAbility)
        {
            
        }
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

    }

}
