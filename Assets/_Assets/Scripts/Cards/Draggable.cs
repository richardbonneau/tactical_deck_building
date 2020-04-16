using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
     public Transform parentToReturnTo;
     public Transform placeholderParent;
    public GameObject placeholderPrefab;
    [System.NonSerialized] public GameObject placeholder;

    [System.NonSerialized] public bool willPlayCard = false;
    [System.NonSerialized] public bool isPlacingAbilityOnCard = false;
    bool isPlayableCard = false;
    bool isAbility = false;

    void Awake()
    {
        if (this.gameObject.CompareTag("Card")) isPlayableCard = true;
        else if (this.gameObject.CompareTag("Ability")) isAbility = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        placeholder = Instantiate(placeholderPrefab, this.transform.position, Quaternion.identity);

        placeholder.transform.SetParent(this.transform.parent);
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        if (this.transform.CompareTag("Ability") && parentToReturnTo.CompareTag("Card") || parentToReturnTo.CompareTag("Inventory")) this.transform.SetParent(this.transform.parent.parent.parent);
        else this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
        print("parentToReturnTo: "+ parentToReturnTo.name);
        if (placeholderParent == null) {
            print(0);
            return;
            };
        if (this.transform.CompareTag("Ability") && placeholderParent.CompareTag("CardDropZone")) {
            print(1);
            return;
            }
        else if (this.transform.CompareTag("Card") && placeholderParent.CompareTag("CardDropZone") || placeholderParent.GetComponent<DropZone>().dropZoneHasCard) {
           print(2);
            return;
            }
        else if (this.transform.CompareTag("Card") && placeholderParent.CompareTag("Card")) {
            print(3);
            return;
            }
        else if (placeholder.transform.parent != placeholderParent) {
            print(4);
            placeholder.transform.SetParent(placeholderParent);
            }
        print(5);
        int newSiblingIndex = placeholderParent.childCount;
        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            // print("-----");
            // print("a "+(isPlayableCard && this.transform.position.x < placeholderParent.GetChild(i).position.x).ToString());
            // print("b "+(isPlayableCard && this.transform.position.y > placeholderParent.GetChild(i).position.y && placeholderParent.transform.CompareTag("Inventory")).ToString());
            // print("c "+(isAbility && this.transform.position.y > placeholderParent.GetChild(i).position.y).ToString());

            if (isPlayableCard && this.transform.position.x < placeholderParent.GetChild(i).position.x 
            || isPlayableCard && this.transform.position.y > placeholderParent.GetChild(i).position.y && placeholderParent.transform.CompareTag("Inventory") 
            || isAbility && this.transform.position.y > placeholderParent.GetChild(i).position.y)
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

        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

    }
}
