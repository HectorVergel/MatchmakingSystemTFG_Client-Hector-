using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private MenuManager m_MenuManager;
    
    [Header("GameTable")] [SerializeField] private Transform m_PlayedCardsTransform;
    [SerializeField] private CardStash m_CardStash;
    private List<Player> m_PlayersInGame = new List<Player>();
    private Player m_SessionPlayer;
    private Match m_MatchData;

    private GameObject m_PlayersHolder;
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
        
        m_MenuManager = FindObjectOfType<MenuManager>();
        if (m_SessionPlayer?.name != "default")
        {
            InitializePlayerInformation(m_SessionPlayer.name, m_SessionPlayer.ranking);
            return;
        }
        InitializePlayerInformation("default", 0000);
    }

    private void Start()
    {
        CalculatePlayersInGame();
       
    }

    private void CalculatePlayersInGame()
    {
        if (m_MatchData == null) return;
        for (int i = 0; i < m_MatchData.players.Length; i++)
        {
            AddPlayer(m_MatchData.players[i]);
        }
    }
    public void AddPlayer(Player _player)
    {
        m_PlayersInGame.Add(_player);
    }

    public void InitializePlayerInformation(string _name, int _ranking)
    {
        m_SessionPlayer = new Player(_name, _ranking);
        RefreshPlayerInformation();
    }

    public void RefreshPlayerInformation()
    {
        m_MenuManager?.RefreshPlayerInformation(m_SessionPlayer);
    }

    public Player GetPlayer()
    {
        return m_SessionPlayer;
    }

    public MenuManager GetMenuManager()
    {
        return m_MenuManager;
    }

    public CardStash GetCardStash()
    {
        return m_CardStash;
    }

    public Transform GetPlayCardTransform()
    {
        return m_PlayedCardsTransform;
    }

    public void InitializeMatch(Match _match)
    {
        m_MatchData = _match;
        CalculatePlayersInGame();
        LoadClientScene("Game_Scene");
    }

    public Match GetMatch()
    {
        return m_MatchData;
    }

    private void LoadClientScene(string _name)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(_name));
    }
    
    IEnumerator LoadSceneAsyncCoroutine(string _name)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_name);

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            Debug.Log("Cargando escena: " + (progress * 100) + "%");

            yield return null; 
        }
        InitializePlayersHUD();

    }

    private void InitializePlayersHUD()
    {
        m_PlayersHolder = GameObject.FindGameObjectWithTag("PLAYERS");
        for (int i = 0; i < m_MatchData.players.Length; i++)
        {
            PlayerGameInfo l_PlayerInfo = m_PlayersHolder.transform.GetChild(i).GetComponent<PlayerGameInfo>();
            l_PlayerInfo.Initialize(m_MatchData.players[i].name, 7);
            l_PlayerInfo.gameObject.SetActive(true);
        }
        
    }

    public void UpdatePlayersCards(string _name, bool _add)
    {
        for (int i = 0; i < m_MatchData.players.Length; i++)
        {
            PlayerGameInfo l_PlayerInfo = m_PlayersHolder.transform.GetChild(i).GetComponent<PlayerGameInfo>();
            if (l_PlayerInfo.GetName() == _name)
            {
                l_PlayerInfo.UpdateCards(_add);
            }
        }
    }
    
    public void CheckIfMatchEnded()
    {
        for (int i = 0; i < m_MatchData.players.Length; i++)
        {
            PlayerGameInfo l_PlayerInfo = m_PlayersHolder.transform.GetChild(i).GetComponent<PlayerGameInfo>();
            if (l_PlayerInfo.GetCards() == 0)
            {
              EndMatch(l_PlayerInfo.GetName());   
            }
        }
    }

    private void EndMatch(string winner)
    {
        Player l_Oponent = m_PlayersInGame.FirstOrDefault(player => player.name != m_SessionPlayer.name);
        if (winner == m_SessionPlayer.name)
        {
            m_SessionPlayer.ranking = EloCalculator.ComputeEloRating(1, m_SessionPlayer.ranking, l_Oponent.ranking);
        }
        else
        {
            m_SessionPlayer.ranking = EloCalculator.ComputeEloRating(0, m_SessionPlayer.ranking, l_Oponent.ranking);
        }
        Debug.Log("NEW RANKING: " + m_SessionPlayer.ranking);
        m_PlayersInGame.Clear();
        SceneManager.LoadScene("Menu_Scene");
    }
}