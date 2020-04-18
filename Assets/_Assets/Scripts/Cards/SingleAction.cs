using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SingleAction : MonoBehaviour
{
    CardsManager cardsManager;
    public string actionType;
    public int value;
    public Image iconImage;
    public TextMeshProUGUI abilityText;
    CardAbilities parentCardAbilities;
    void Awake()
    {
        cardsManager = GameObject.FindWithTag("CardsManager").GetComponent<CardsManager>();
        abilityText.text = WriteAbilityName(actionType, value);
        if (this.transform.parent.CompareTag("Card"))
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            parentCardAbilities = this.transform.parent.GetComponent<CardAbilities>();
        }

    }
    public void PutAbilityOnParentCard()
    {
        if (this.transform.parent.CompareTag("Card")) parentCardAbilities = this.transform.parent.GetComponent<CardAbilities>();
        if (parentCardAbilities != null)
        {
            parentCardAbilities.actionsTypes.Add(actionType);
            parentCardAbilities.actionsValues.Add(value);
        }
    }

    string WriteAbilityName(string type, int val)
    {
        switch (type)
        {
            case "move":
                {
                    iconImage.sprite = cardsManager.abilityIcons[0];
                    return "Move " + val;
                    break;
                }

            case "attack":
                {
                    iconImage.sprite = cardsManager.abilityIcons[1];
                    return "Melee Attack " + val;
                    break;
                }

            case "heal":
                {
                    iconImage.sprite = cardsManager.abilityIcons[2];
                    return "Heal Self " + val;
                    break;
                }
            case "area":
                {
                    iconImage.sprite = cardsManager.abilityIcons[3];
                    return "Ranged AoE Attack " + val;
                    break;
                }
            case "shield":
                {
                    iconImage.sprite = cardsManager.abilityIcons[4];
                    return "Ignore Next Source of Damage";
                    break;
                }

            default:
                print("Ability Type not found");
                return type + " " + val;
                break;
        }
    }
}
