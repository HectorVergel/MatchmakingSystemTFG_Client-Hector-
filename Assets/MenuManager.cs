using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MenuManager : MonoBehaviour
{
    [SerializeField] Console m_Console;

    [SerializeField] private TextMeshProUGUI m_PlayerName;
    [SerializeField] private TextMeshProUGUI m_Rank;

    [SerializeField] private GameObject m_WaitingUI;
    // Update is called once per frame
    void Update()
    {
            ConsoleUpdate();
    }

    private void ConsoleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            m_Console.gameObject.SetActive(!m_Console.gameObject.activeSelf);
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