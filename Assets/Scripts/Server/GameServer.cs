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
        Debug.Log("CARD SUCCES " + m_CardSucces + " CARDINFO " + m_LastCardInfo);
        if (m_CardSucces && m_LastCardInfo != null)
        {
            CardDealer.instance.PlayCard(m_LastCardInfo);
            m_CardSucces = false;
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
        else
        {
            m_isMyTurn = true;
        }
    }
}