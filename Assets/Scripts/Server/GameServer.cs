using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;
using Random = UnityEngine.Random;
using Newtonsoft.Json;

public class GameServer : MonoBehaviour
{
    private WebSocket ws;
    public static GameServer instance;
    private bool m_CardSucces = false;
    private CardInfo m_LastCardInfo;
    public bool m_isMyTurn;
    public GameObject m_TurnGUI;
    private string m_LastPlayerName;
    private bool m_UpdateCardsSubstract;
    private bool m_UpdateCardsAdd;

    private void Awake()
    {
        ws = new WebSocket("ws://localhost:4000");
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        ws.OnOpen += OnConnectToWSS;
        ws.OnMessage += OnMessage;
        ws.ConnectAsync();
    }

    private void Update()
    {
        if (m_CardSucces && m_LastCardInfo != null)
        {
            Debug.Log(GameManager.instance.GetPlayer().name+" CARD PLAYED");
            CardDealer.instance.PlayCard(m_LastCardInfo);
            m_CardSucces = false;
        }

        if (m_UpdateCardsSubstract)
        {
            GameManager.instance.UpdatePlayersCards(m_LastPlayerName, false);
            m_UpdateCardsSubstract = false;
        }

        if (m_UpdateCardsAdd)
        {
            GameManager.instance.UpdatePlayersCards(m_LastPlayerName, true);
            Debug.Log("CARD ADDED");
            m_UpdateCardsAdd = false;
        }

        m_TurnGUI.SetActive(!m_isMyTurn);
    }
    public void ApplyOtherEffect()
    {
        if (m_LastCardInfo == null)
            return;
        
        switch (m_LastCardInfo.type)
        {
            case CARD_TYPE.SUM2:
                m_LastCardInfo.effect = FindObjectOfType<Effect_Sum2>();
                break;
            case CARD_TYPE.SUM4:
                m_LastCardInfo.effect = FindObjectOfType<Effect_Sum4>();
                break;
        }

        if (m_LastCardInfo.effect)
        {
            m_LastCardInfo.effect.DoEffect(m_LastCardInfo);
        }
    }
    public void SendCardPlayed(CardInfo _cardInfo)
    {
        string l_jsonData = JsonUtility.ToJson(_cardInfo, true);
        ws.Send($"JSON${l_jsonData}${GameManager.instance.GetMatch().id}${GameManager.instance.GetPlayer().name}");
        if (_cardInfo.type != CARD_TYPE.BLOCK && _cardInfo.type != CARD_TYPE.SWITCH)
        {
            m_isMyTurn = false;
            m_TurnGUI.SetActive(true);
        } 
    }

    public void SendSteal()
    {
        ws.Send($"STEAL${GameManager.instance.GetPlayer().name}${GameManager.instance.GetMatch().id}$STEAL");
    }

    private void OnConnectToWSS(object sender, EventArgs e)
    {
        ws.Send($"INFO${GameManager.instance.GetMatch().id}${GameManager.instance.GetPlayer().name}$extra");
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("MENSAJE:" + e.Data);
        if (e.Data.Contains("color"))
        {
            try
            {
                m_LastCardInfo = JsonConvert.DeserializeObject<CardInfo>(e.Data);
                Debug.Log("Deserializaci�n exitosa. Match ID: " + m_LastCardInfo.color.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogError("Error durante la deserializaci�n: " + ex.Message);
            }

            m_CardSucces = true;
        }
        else if (e.Data.Contains("steal"))
        {
            m_UpdateCardsAdd = true;
            m_LastPlayerName = e.Data.Split('_')[0];
        }
        else if (e.Data.Contains("block"))
        {
            m_UpdateCardsSubstract = true;
            m_LastPlayerName = e.Data.Split('_')[1];
        }
        else
        {
            m_isMyTurn = true;
            m_UpdateCardsSubstract = true;
            m_LastPlayerName = e.Data;
        }
    }
}