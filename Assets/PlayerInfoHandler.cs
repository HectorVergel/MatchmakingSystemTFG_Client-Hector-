using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_NameInputField;
    [SerializeField] private TMP_InputField m_RankInputField;
    [SerializeField] private Button m_AcceptButton;

    public void CreatePlayerInfo()
    {
        int l_Ranking = m_RankInputField == null ? EloCalculator.m_InitialRanking : int.Parse(m_RankInputField.text);
        GameManager.instance.InitializePlayerInformation(m_NameInputField.text, l_Ranking);
        GameManager.instance.RefreshPlayerInformation();
    }

    public void SetNewRank(int rank)
    {
        GameManager.instance.InitializePlayerInformation(m_NameInputField.text, rank);
        GameManager.instance.RefreshPlayerInformation();
    }
}