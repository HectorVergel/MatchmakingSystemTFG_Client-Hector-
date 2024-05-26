using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogIn : MonoBehaviour
{
    [SerializeField] private GameObject m_Log;
    void Start()
    {
        if (GameManager.instance.GetPlayer().name != "default")
        {
            m_Log.gameObject.SetActive(false);
        }
    }
    
}
