using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MenuManager : MonoBehaviour
{
    [SerializeField] PlayerInfoHandler m_PlayerInfoHandler;

    [SerializeField] private TextMeshProUGUI m_PlayerName;
    [SerializeField] private TextMeshProUGUI m_Rank;

    [SerializeField] private GameObject m_WaitingUI;
    // Update is called once per frame
    private void Start()
    {
        if (GameManager.instance.GetPlayer() != null)
        {
            m_PlayerInfoHandler?.SetNewRank(GameManager.instance.GetPlayer().ranking);
        }
        
    }

    void Update()
    {
            ConsoleUpdate();
    }

    private void ConsoleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            m_PlayerInfoHandler.gameObject.SetActive(!m_PlayerInfoHandler.gameObject.activeSelf);
        }
    }

    public void RefreshPlayerInformation(Player _player)
    {
        m_PlayerName.text = _player.name;
        m_Rank.text = _player.ranking.ToString();

    }

    public void ActivateWaitingUI()
    {
        m_WaitingUI.SetActive(true);
    }
}