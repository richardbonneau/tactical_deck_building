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
        abilityText.text = actionType + " " + value;
        if(this.transform.parent.name == "Card")  parentCardAbilities = this.transform.parent.GetComponent<CardAbilities>();
    }
    public void PutAbilityOnParentCard(){
if(this.transform.parent.name == "Card")  parentCardAbilities = this.transform.parent.GetComponent<CardAbilities>();
       if(parentCardAbilities != null){
        parentCardAbilities.actionsTypes.Add(actionType);
        parentCardAbilities.actionsValues.Add(value);
       }
    }
}
