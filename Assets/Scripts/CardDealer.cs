using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CardDealer : MonoBehaviour
{
    public static CardDealer instance;
    
    [Header("Cards")] 
    [SerializeField] private Card m_CardPrefab;
    [SerializeField] private int m_MaxCardsPerPlayer = 7;
    [SerializeField] private float m_SecondsToStart = 2.0f;
    [SerializeField] private float m_SecondsBetweenCards = 0.1f;

    [Header("References")] [SerializeField]
    private List<Transform> m_PlayersHandsTransforms = new List<Transform>();
    [SerializeField] private Transform m_PlayerHand;

    private Transform m_PlayedCards;
    private int m_PlayersNum;

    private Card m_LastCardPlayed;

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
    }

    private void Start()
    {
        m_PlayedCards = GameManager.instance.GetPlayCardTransform();
        m_PlayersNum = GameManager.instance.GetNumberOfPlayers();
        GiveCardToCurrentPlayer();
        InstantiateCard(m_PlayedCards.gameObject.transform, true);
    }

    public void GiveCardToCurrentPlayer()
    {
        StartCoroutine(GiveCards());
    }

    public void StealCard()
    {
        InstantiateCard(m_PlayerHand);
    }
    private IEnumerator GiveCards()
    {
        yield return new WaitForSeconds(m_SecondsToStart);

        for (int j = 0; j < m_MaxCardsPerPlayer; j++)
        {
            InstantiateCard(m_PlayerHand);
            yield return new WaitForSeconds(m_SecondsBetweenCards);
        }
    }

    private void InstantiateCard(Transform _parent, bool _isLastCard = false)
    {
        Card l_Card = Instantiate(m_CardPrefab, _parent);
        l_Card.Initialize(CardDeck.instance.GetAvailiableCard(_isLastCard));
        l_Card.m_CardInfo.isOnDeck = false;

        if (_isLastCard)
        {
            SetLastCardPlayed(l_Card);
        }

    }
    
    public void SetLastCardPlayed(Card _card)
    {
        if (m_LastCardPlayed != null)
        {
            m_LastCardPlayed.m_CardInfo.isOnDeck = true;
            Destroy(m_LastCardPlayed.gameObject, 1f);

        }
        m_LastCardPlayed = _card;
    }

    public Card GetLastCardPlayed()
    {
        return m_LastCardPlayed;
    }
}