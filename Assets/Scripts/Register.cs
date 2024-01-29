using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_nameField;
    [SerializeField] private TMP_InputField m_rankingField;

    public void SendInformation()
    {
        GameManager.m_instance.CreatePlayerInformation(m_nameField.text, m_rankingField.text);
    }
}
