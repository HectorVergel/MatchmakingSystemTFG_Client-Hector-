using UnityEngine;

public class Effect_ChooseColor : Effect
{
    [SerializeField] private GameObject m_ChooseColorUI;
    public override void DoEffect()
    {
        m_ChooseColorUI.SetActive(true);   
    }
    
    public void ChangeColor(string _color)
    {
        CARD_COLOR l_Color = (CARD_COLOR)System.Enum.Parse(typeof(CARD_COLOR), _color);
        CardDealer.instance.GetLastCardPlayed().m_CardInfo.color = l_Color;
    }
}