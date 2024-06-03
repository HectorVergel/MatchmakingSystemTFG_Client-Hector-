using UnityEngine;

public class TurnChangedHandler : MonoBehaviour
{
    private void OnDisable()
    {
        Debug.Log("Disabled");
        GameServer.instance.ApplyOtherEffect();
    }
    
    
}