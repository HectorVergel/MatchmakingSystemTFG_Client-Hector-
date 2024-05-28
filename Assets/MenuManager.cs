using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;


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
            //m_PlayerInfoHandler.gameObject.SetActive(!m_PlayerInfoHandler.gameObject.activeSelf);
        }
    }

    public void RefreshPlayerInformation(string _name, int _rank)
    {
        m_PlayerName.text = _name;
        m_Rank.text = _rank.ToString();

    }
    
}