using TMPro;
using UnityEngine;

public class PlayerInformationGUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_name;
    [SerializeField] TextMeshProUGUI m_ranking;
    [SerializeField] TextMeshProUGUI m_team;


    public void InitPlayerInformation(string _name, string _ranking, int team)
    {
        m_name.text = _name;
        m_ranking.text = _ranking;
        m_team.text = "Team: " + team.ToString();
    }
}
