using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDeck : MonoBehaviour
{
    [SerializeField] private List<CardInfo> m_CardsInDeck;
    public static CardDeck instance;


    public void Initialize()
    {
        FillDeck();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        Initialize();
    }

    public CardInfo GetAvailiableCard(bool _isLastCard = false)
    {
        CardInfo l_CardInfo = m_CardsInDeck[Random.Range(0, m_CardsInDeck.Count)];
        if (!_isLastCard)
        {
            if (l_CardInfo.isOnDeck)
            {
                return l_CardInfo;
            }
        }
        else
        {
            if (l_CardInfo.isOnDeck && l_CardInfo.type == CARD_TYPE.NUMBER)
            {
                return l_CardInfo;
            }
        }
        

        return GetAvailiableCard();
    }

    private void FillDeck()
    {
        Sprite[] all = Resources.LoadAll<Sprite>("UNO_Front");

        foreach (var s in all)
        {
            string[] l_SplitName = s.name.Split('_');


            CARD_TYPE type;
            CARD_COLOR color;
            CardInfo l_CardInfo;

            if (l_SplitName[0] == "SUM4" ||l_SplitName[0] == "COLOR")
            {
                type = (CARD_TYPE)System.Enum.Parse(typeof(CARD_TYPE), l_SplitName[0]);
                l_CardInfo = new CardInfo(type, CARD_COLOR.NONE);
                l_CardInfo.effect = FindObjectOfType<Effect_ChooseColor>();
                m_CardsInDeck.Add(l_CardInfo);
            }
            else if (l_SplitName[0] == "SUM2" || l_SplitName[0] == "SWITCH" || l_SplitName[0] == "BLOCK")
            {
                type = (CARD_TYPE)System.Enum.Parse(typeof(CARD_TYPE), l_SplitName[0]);
                color = (CARD_COLOR)System.Enum.Parse(typeof(CARD_COLOR), l_SplitName[1]);
                l_CardInfo = new CardInfo(type, color);
                m_CardsInDeck.Add(l_CardInfo);
            }
            // else if (l_SplitName[0] == "SUM2" || l_SplitName[0] == "SUM4")
            // {
            //     return;
            // }
            else
            {
                int number = int.Parse(l_SplitName[0]);
                color = (CARD_COLOR)System.Enum.Parse(typeof(CARD_COLOR), l_SplitName[1]);
                l_CardInfo = new CardInfo(CARD_TYPE.NUMBER, color, number);
                m_CardsInDeck.Add(l_CardInfo);
            }

            l_CardInfo.sprite = GetSpriteFromInfo(l_CardInfo);
        }
    }

    public Sprite GetSpriteFromInfo(CardInfo _cardInfo)
    {
        Sprite l_CardSprite;
        if (_cardInfo.type == CARD_TYPE.NUMBER)
        {
            l_CardSprite = Load(_cardInfo.number.ToString() + "_" + _cardInfo.color.ToString());
        }
        else if ((_cardInfo.type == CARD_TYPE.COLOR) || (_cardInfo.type == CARD_TYPE.SUM4))
        {
            l_CardSprite = Load(_cardInfo.type.ToString() + "_" + "1");
        }
        else
        {
            l_CardSprite = Load(_cardInfo.type.ToString() + "_" + _cardInfo.color.ToString());
        }

        return l_CardSprite;
    }

    private Sprite Load(string spriteName)
    {
        Sprite[] all = Resources.LoadAll<Sprite>("UNO_Front");

        foreach (var s in all)
        {
            if (s.name == spriteName)
            {
                return s;
            }
        }

        return null;
    }
}