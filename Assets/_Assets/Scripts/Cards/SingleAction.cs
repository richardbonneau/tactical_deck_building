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
        abilityText.text = WriteAbilityName(actionType,value);
        if(this.transform.parent.CompareTag("Card")) {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            parentCardAbilities = this.transform.parent.GetComponent<CardAbilities>();
        } 

    }
    public void PutAbilityOnParentCard(){
        if(this.transform.parent.CompareTag("Card"))  parentCardAbilities = this.transform.parent.GetComponent<CardAbilities>();
        if(parentCardAbilities != null){
        parentCardAbilities.actionsTypes.Add(actionType);
        parentCardAbilities.actionsValues.Add(value);
       }
    }

    string WriteAbilityName(string type, int val){
        switch (type)
        {
          case "move":
              return "Move "+val;
              break;
          case "attack":
              return "Melee Attack "+val;
              break;
          default:
              print("Ability Type not found");
              return type+" "+val;
              break;
        }
    }
}
