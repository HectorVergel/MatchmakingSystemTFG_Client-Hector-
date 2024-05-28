using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogIn : MonoBehaviour
{
    [SerializeField] private GameObject m_Log;
    void Start()
    {
        if (PlayerPrefs.GetString("SessionName") != "default")
        {
            m_Log.gameObject.SetActive(false);
        }
    }
    
}
