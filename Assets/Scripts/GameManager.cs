using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager m_instance;
    
    [Header("References")]
    [SerializeField] private PlayerController m_playerController;
    private float m_score;
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(m_instance);
        }

        DontDestroyOnLoad(this.gameObject);
    }


    #region GETTTERS AND SETTERS
    public PlayerController GetPlayerController()
    {
        return m_playerController;
    }

    public void SetScore(float _score)
    {
        m_score = _score;
    }

    public float GetScore()
    {
        return m_score;
    }

    #endregion

}
