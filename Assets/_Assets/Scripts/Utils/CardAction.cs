using System.Collections;
using UnityEngine;

public class CardAction
{
    public string actionType;
    public int value;


    public CardAction(string _actionType, int _value)
    {
        actionType = _actionType;
        value = _value;
    }
}
