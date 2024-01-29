using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{

    public static GameManager m_instance;
    
    [Header("References")]
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private GameObject m_mainMenu;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI m_playerNameUI;
    [SerializeField] private TextMeshProUGUI m_playerRankingUI;
    [SerializeField] private GameObject m_waitingTextUI;

    private Player m_player;
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


    //GAME CLIENT SIDE CODE ---------------------------------------------
    public void UpdatePlayerMenuInformation()
    {
        m_playerNameUI.text = m_player.name;
        m_playerRankingUI.text = m_player.ranking.ToString();
    }

    //SERVER SIDE CODE ---------------------------------------------
    public void CreatePlayerInformation(string _name, string _ranking)
    {       
        m_player = new Player(_name, float.Parse(_ranking));
    }
    
    public void SendMatchRequest(string _gameMode)
    {
        StartCoroutine(SendPlayerRequestToServerCoroutine(_gameMode));
    }

    IEnumerator SendPlayerRequestToServerCoroutine(string _gameMode)
    {
       

        string jsonData = JsonUtility.ToJson(m_player, true);
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        Debug.Log(jsonData);

        string serverUrl = "http://localhost:3000"; // Ruta para enviar datos al servidor

        UnityWebRequest request = new UnityWebRequest(serverUrl + $"/{_gameMode}", "POST");
        UploadHandlerRaw uploadHandler = new UploadHandlerRaw(postData);
        uploadHandler.contentType = "application/json";
        request.uploadHandler = uploadHandler;
        request.downloadHandler = new DownloadHandlerBuffer();

       

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos enviados al servidor correctamente");

            // Procesar la respuesta del servidor
            //ProcesarRespuestaDelServidor(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error al enviar datos al servidor: " + request.error);
        }
    }

}
