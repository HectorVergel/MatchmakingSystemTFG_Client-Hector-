using System;
using UnityEngine;

[Serializable]
public class CardInfo
{
    public CARD_COLOR color;
    public CARD_TYPE type;
    public int number;
    [NonSerialized]public bool isOnDeck;
    [NonSerialized]public Sprite sprite;
    [NonSerialized]public Effect effect;
    
    public CardInfo(CARD_TYPE type, CARD_COLOR color, int number = -1)
    {
        this.type = type;
        this.color = color;
        this.number = number;
        isOnDeck = true;
    }

}