using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class GUI_Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    
    [SerializeField] private float m_HoverScale = 1.25f;
    [SerializeField] private float m_PositionOffset = 1.25f;
    [SerializeField] private Card m_Card;
    private HorizontalLayoutGroup m_MyLayout;
    private RectTransform m_RectTransform;
    private Vector3 m_OriginalScale;
    private Vector2 m_OriginalPosition;
    private Canvas m_ParentCanvas;
    
    
    private bool m_IsHovering;
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        
        m_OriginalScale = m_RectTransform.localScale;
        m_OriginalPosition = m_RectTransform.localPosition;
        m_MyLayout = GameObject.FindGameObjectWithTag("HAND").GetComponent<HorizontalLayoutGroup>();
        m_ParentCanvas = transform.parent.GetComponent<Canvas>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_IsHovering = true;
        // Increase the scale of the Image
        m_ParentCanvas.overrideSorting = true;
        m_ParentCanvas.sortingOrder = 100;
        m_MyLayout.enabled = false;
        m_RectTransform.localScale = m_OriginalScale * m_HoverScale;
        m_RectTransform.localPosition = new Vector2(m_OriginalPosition.x, m_OriginalPosition.y + m_PositionOffset);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_Card.PlayCard();
    }

    // Called when the mouse exits the image
    public void OnPointerExit(PointerEventData eventData)
    {
        m_IsHovering = false;
        // Reset the scale of the Image
        m_ParentCanvas.overrideSorting = false;
        m_ParentCanvas.sortingOrder = 0;
        m_MyLayout.enabled = true;
        m_RectTransform.localScale = m_OriginalScale;
        m_RectTransform.localPosition = m_OriginalPosition;
    }
}