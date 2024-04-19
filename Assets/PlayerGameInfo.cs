using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_PlayerName;
    [SerializeField] private TextMeshProUGUI m_Ranking;
    [SerializeField] private Image m_ProfilePic;

    public void Initialize(string _playerName, int _rank)
    {
        m_PlayerName.text = _playerName;
        m_Ranking.text = _rank.ToString();
    }
}
