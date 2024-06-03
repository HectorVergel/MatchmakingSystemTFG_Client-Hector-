using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Card : MonoBehaviour
{
    public CardInfo m_CardInfo;
    private SpriteRenderer m_Renderer;

    [SerializeField] private RectTransform m_RectTransform;
    [SerializeField] private Image m_Image;
    [SerializeField] private float m_MoveSpeed = 10.0f;

    public void Initialize(CardInfo _cardInfo)
    {
        m_CardInfo = _cardInfo;
        if (m_CardInfo.sprite == null)
        {
            m_Image.sprite = CardDeck.instance.GetSpriteFromInfo(_cardInfo);
        }
        else
        {
            m_Image.sprite = m_CardInfo.sprite;
        }
    }

    public void PlayCard()
    {
        //CHECK IF CAN PLAY CARD
        if (CanPlayCardSelected() && GameServer.instance.m_isMyTurn)
        {
            //MOVE TO PLAYED CARDS
            transform.SetParent(GameManager.instance.GetPlayCardTransform());
            StartCoroutine(MoveToPosition(Vector3.zero));
            CardDealer.instance.SetLastCardPlayed(this);
            if (m_CardInfo.effect is Effect_ChooseColor)
            {
                m_CardInfo.effect.DoEffect(m_CardInfo);
            }
            else
            {
                GameServer.instance.SendCardPlayed(m_CardInfo);
            }

            GameManager.instance.UpdatePlayersCards(GameManager.instance.GetPlayer().name, false);
        }
    }

    private bool CanPlayCardSelected()
    {
        CardInfo l_LastCard = CardDealer.instance.GetLastCardPlayed().m_CardInfo;
        if (m_CardInfo.type == CARD_TYPE.SUM4 || m_CardInfo.type == CARD_TYPE.COLOR)
        {
            return true;
        }

        if (m_CardInfo.color == l_LastCard.color)
        {
            return true;
        }

        if (m_CardInfo.type != CARD_TYPE.NUMBER && m_CardInfo.type == l_LastCard.type)
        {
            return true;
        }

        if (m_CardInfo.type == CARD_TYPE.NUMBER && m_CardInfo.number == l_LastCard.number)
        {
            return true;
        }
        return false;
    }

    private IEnumerator MoveToPosition(Vector3 _target)
    {
        while (Vector2.Distance(m_RectTransform.anchoredPosition, _target) > 0.01f)
        {
            Vector2 newPosition = Vector2.Lerp(m_RectTransform.anchoredPosition, _target, Time.deltaTime * m_MoveSpeed);
            m_RectTransform.anchoredPosition = newPosition;

            yield return null;
        }
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
    GREEN,
    NONE
}