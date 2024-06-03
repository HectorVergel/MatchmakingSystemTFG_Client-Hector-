using System.Collections;
using UnityEngine;

public class Effect_ChooseColor : Effect
{
    [SerializeField] private GameObject m_ChooseColorUI;
    public override void DoEffect(CardInfo _info)
    {
        m_ChooseColorUI.SetActive(true);
        StartCoroutine(ColorCoroutine(_info));
    }
    
    public void ChangeColor(string _color)
    {
        CARD_COLOR l_Color = (CARD_COLOR)System.Enum.Parse(typeof(CARD_COLOR), _color);
        CardDealer.instance.GetLastCardPlayed().m_CardInfo.color = l_Color;
    }

    IEnumerator ColorCoroutine(CardInfo _info)
    {
        CARD_COLOR l_LastColor = CardDealer.instance.GetLastCardPlayed().m_CardInfo.color;
        while (l_LastColor == CARD_COLOR.NONE)
        {
            l_LastColor = CardDealer.instance.GetLastCardPlayed().m_CardInfo.color;
            yield return null;
        }
        
        GameServer.instance.SendCardPlayed(_info);
    }
}