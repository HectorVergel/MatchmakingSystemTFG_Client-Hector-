using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class ServerManager : MonoBehaviour
{
    [SerializeField] private float m_TimeToUpdate = 5.0f;

    public static ServerManager m_instance;
    private Match m_Match;
    private Player m_Player;
    private float m_Timer = 0.0f;
    private bool m_SearchingMatch = false;
    private string m_CurrentGameMode;

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

    private void Update()
    {
        if (m_SearchingMatch)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_TimeToUpdate)
            {
                TryToStartMatch();
                m_Timer = 0.0f;
            }
        }
    }

    //SERVER SIDE CODE ---------------------------------------------
    //MATCH --------------------
    public void SendMatchRequest(string _gameMode)
    {
        m_SearchingMatch = true;
        StartCoroutine(POSTFindMatch(_gameMode));
    }

    public void LeaveQueueRequest(string _gameMode)
    {
        m_SearchingMatch = false;
        StartCoroutine(DELETEPlayerFromQueue(_gameMode));
    }

    IEnumerator POSTFindMatch(string _gameMode)
    {
        m_CurrentGameMode = _gameMode;
        Player l_PlayerData = GameManager.instance.GetPlayer();
        string l_jsonData = JsonUtility.ToJson(l_PlayerData, true);
        byte[] l_postData = Encoding.UTF8.GetBytes(l_jsonData);

        Debug.Log(l_jsonData);

        string l_serverUrl = "http://localhost:3000";

        UnityWebRequest l_request = new UnityWebRequest(l_serverUrl + $"/find-match/{_gameMode}", "POST");
        UploadHandlerRaw l_uploadHandler = new UploadHandlerRaw(l_postData);

        l_uploadHandler.contentType = "application/json";
        l_request.uploadHandler = l_uploadHandler;
        l_request.downloadHandler = new DownloadHandlerBuffer();


        yield return l_request.SendWebRequest();

        if (l_request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos enviados al servidor correctamente");

        }
        else
        {
            Debug.LogError("Error al enviar datos al servidor: " + l_request.error);
        }
    }

    private void TryToStartMatch()
    {
        StartCoroutine(GETMatch(m_CurrentGameMode));
       
    }

    IEnumerator GETMatch(string _gameMode)
    {
        string serverUrl = "http://localhost:4000"; // URL del servidor
        string requestUrl = $"{serverUrl}/match/{GameManager.instance.GetPlayer().name}";

        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            // Enviar la solicitud y esperar la respuesta
            yield return request.SendWebRequest();

            // Verificar errores
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Procesar la respuesta

                Debug.Log("Datos recibidos del servidor correctamente");
                string response = request.downloadHandler.text;
                Debug.Log("Respuesta del servidor: " + response);
                // Puedes realizar aquí cualquier acción adicional con la respuesta recibida
                ProcessServerRes(response);
            }
            else
            {
                Debug.LogError("Error al recibir datos del servidor: " + request.error);
            }
        }
    }

    IEnumerator POSTMatchmaker()
    {
        string l_serverUrl = "http://localhost:3000";

        UnityWebRequest l_request = new UnityWebRequest(l_serverUrl + $"/matchmaker", "POST");

        l_request.downloadHandler = new DownloadHandlerBuffer();
        yield return l_request.SendWebRequest();

        if (l_request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos enviados al servidor correctamente");
            //GameManager.instance.GetMenuManager().ActivateWaitingUI();
            m_SearchingMatch = true;
        }
        else
        {
            Debug.LogError("Error al enviar datos al servidor: " + l_request.error);
        }
    }

    IEnumerator DELETEPlayerFromQueue(string _gameMode)
    {
        string serverUrl = "http://localhost:3000"; // URL del servidor
        string playerName = GameManager.instance.GetPlayer().name;
        string requestUrl = $"{serverUrl}/queue/{_gameMode}/{playerName}";

        using (UnityWebRequest request = UnityWebRequest.Delete(requestUrl))
        {
            // Enviar la solicitud y esperar la respuesta
            yield return request.SendWebRequest();

            // Verificar errores
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Eliminación exitosa del servidor para el jugador: " + playerName);
            }
            else
            {
                Debug.LogError("Error al eliminar datos del servidor para el jugador " + playerName + ": " +
                               request.error);
            }
        }
    }

    private void ProcessServerRes(string _res)
    {
        Debug.Log("JSON antes de deserializar: " + _res.Trim('"'));
        _res = _res.Trim('"');
        _res = _res.Replace("\\\"", "\"");
        try
        {
            Match l_Match = JsonConvert.DeserializeObject<Match>(_res);
            Debug.Log("Deserializaci�n exitosa. Match ID: " + l_Match.id);
            m_SearchingMatch = false;
            GameManager.instance.InitializeMatch(l_Match);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error durante la deserializaci�n: " + ex.Message);
        }
    }
}