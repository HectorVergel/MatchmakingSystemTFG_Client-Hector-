using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player 
{
    public int id;
    public string name;
    public int ranking;
    public int timeWaiting;
    public string custom;
    public int team;
    public int maxRankDif;
    public Player()
    {

    }
    public Player(string _name, int _ranking)
    {
        name = _name;
        ranking = _ranking;
        timeWaiting = 0;
        id = 1;
        team = -1;
        maxRankDif = 200;

    }

}
