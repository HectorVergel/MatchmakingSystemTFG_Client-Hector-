using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Console : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_NameInputField;
    [SerializeField] private TMP_InputField m_RankInputField;
    [SerializeField] private Button m_AcceptButton;

    public void CreatePlayerInfo()
    {
        GameManager.instance.InitializePlayerInformation(m_NameInputField.text, int.Parse(m_RankInputField.text));
        GameManager.instance.RefreshPlayerInformation();
    }
}