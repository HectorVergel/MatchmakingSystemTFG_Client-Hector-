using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_PlayerName;
    [SerializeField] private TextMeshProUGUI m_CardsText;
    [SerializeField] private Image m_ProfilePic;

    private int m_CurrentCards;

    public void Initialize(string _playerName, int _cards)
    {
        m_CurrentCards = _cards;
        m_PlayerName.text = _playerName;
        m_CardsText.text = m_CurrentCards.ToString();
    }

    public void UpdateCards(bool _add)
    {
        if (_add)
        {
            m_CurrentCards++;
            Debug.Log("IN");
        }
        else
        {
            m_CurrentCards--;
        }

        m_CardsText.text = m_CurrentCards.ToString();
    }

    public string GetName()
    {
        return m_PlayerName.text;
    }

    public int GetCards()
    {
        return m_CurrentCards;
    }
}