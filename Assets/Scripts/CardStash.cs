using UnityEngine;
using UnityEngine.EventSystems;

public class CardStash : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject m_InfoText;
   

    public void OnPointerClick(PointerEventData eventData)
    {
        CardDealer.instance.StealCard();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_InfoText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_InfoText.SetActive(false);
    }
}