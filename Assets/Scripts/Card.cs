using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Card : MonoBehaviour
{
    private CARD_TYPE m_CardType;
    private CARD_COLOR m_CardColor;
    private int m_CardNumber;
    [SerializeField] private Sprite m_CardBack;
    
     private SpriteRenderer m_Renderer;
     [SerializeField]private Image m_Image;
    
    public void Initialize(CARD_TYPE _type, CARD_COLOR _color)
    {
        
        m_CardType = _type;
        m_CardColor = _color;
        Sprite l_CardSprite;
        if (_type == CARD_TYPE.NUMBER)
        {
            InitCardWithRandomNumber();
            l_CardSprite = Load("UNO_Front", m_CardNumber.ToString() + "_" + m_CardColor.ToString());
        }
        else if (_type == CARD_TYPE.COLOR || _type == CARD_TYPE.SUM4)
        {
            l_CardSprite = Load("UNO_Front", m_CardType.ToString() + "_" + "1");
        }
        else
        {
            l_CardSprite = Load("UNO_Front", m_CardType.ToString()  + "_" + m_CardColor.ToString());
        }
       
        m_Image.sprite = l_CardSprite;

    }

    public void PlayCard()
    {
        
    }

    public void Flip()
    {
        m_Renderer.sprite = m_CardBack;
    }

    private void InitCardWithRandomNumber()
    {
        m_CardNumber = Random.Range(1, 10);
    }
    
    
    Sprite Load( string imageName, string spriteName)
    {
        Sprite[] all = Resources.LoadAll<Sprite>( imageName);
 
        foreach( var s in all)
        {
            if (s.name == spriteName)
            {
                return s;
            }
        }
        return null;
    }
}

public enum CARD_TYPE
{
   NUMBER,
   SWITCH,
   BLOCK,
   SUM2,
   SUM4,
   COLOR
    
}

public enum CARD_COLOR
{
    RED,
    BLUE,
    YELLOW,
    GREEN
}