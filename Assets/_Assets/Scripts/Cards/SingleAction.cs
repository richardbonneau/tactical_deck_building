using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SingleAction : MonoBehaviour
{
    public string actionType;
    public int value;
    // public icon
    public TextMeshProUGUI abilityText;
    CardAbilities parentCardAbilities;


    

    void Awake()
    {
        parentCardAbilities = this.transform.parent.GetComponent<CardAbilities>();
    PutAbilityOnParentCard();
    }

    public void PutAbilityOnParentCard(){
        abilityText.text = actionType+" "+value;
        parentCardAbilities.actionsTypes.Add(actionType);
        parentCardAbilities.actionsValues.Add(value);
    }
}
