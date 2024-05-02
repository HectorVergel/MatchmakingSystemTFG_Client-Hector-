using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameServer : MonoBehaviour
{
    public static GameServer instance;

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

    public void SendCardPlayed(CardInfo _cardInfo)
    {
        
    }
}
