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
            m_UpdateCardsAdd = false;
        }
        
        m_TurnGUI.SetActive(!m_isMyTurn);
    }

    public void SendCardPlayed(CardInfo _cardInfo)
    {
        string l_jsonData = JsonUtility.ToJson(_cardInfo, true);
        ws.Send($"JSON${l_jsonData}${GameManager.instance.GetMatch().id}${GameManager.instance.GetPlayer().name}");
        m_isMyTurn = false;
        m_TurnGUI.SetActive(true);
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
        else
        {
            Debug.Log("Nombre del jugador: " + e.Data);
            m_isMyTurn = true;
            m_UpdateCardsSubstract = true;
            m_LastPlayerName = e.Data;

        }
    }
}