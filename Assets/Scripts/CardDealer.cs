using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CardDealer : CardsHandlerBase
{
    [Header("Cards")] [SerializeField] private Card m_CardPrefab;
    [SerializeField] private int m_MaxCardsPerPlayer = 7;
    [SerializeField] private float m_SecondsToStart = 2.0f;

    [Header("References")] [SerializeField]
    private List<Transform> m_PlayersHandsTransforms = new List<Transform>();

    [SerializeField] private Transform m_PlayerHand;
 
    private int m_PlayersNum;

    private void Start()
    {
        m_PlayersNum = GameManager.instance.GetNumberOfPlayers();
        StartCoroutine(GiveCards());
    }

    private IEnumerator GiveCards()
    {
        yield return new WaitForSeconds(m_SecondsToStart);

        for (int j = 0; j < m_MaxCardsPerPlayer; j++)
        {
            Card l_Card = Instantiate(m_CardPrefab, m_PlayerHand);
            l_Card.Initialize(GetRandomEnum<CARD_TYPE>(), GetRandomEnum<CARD_COLOR>());
            yield return new WaitForSeconds(0.5f);
        }
    }


    private T GetRandomEnum<T>()
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException("Type parameter T must be an enum type.");
        }

        Array enumValues = Enum.GetValues(typeof(T));

        int randomIndex = UnityEngine.Random.Range(0, enumValues.Length);

        return (T)enumValues.GetValue(randomIndex);
    }
}