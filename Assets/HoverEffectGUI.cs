using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverEffectGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float m_HoverScale = 1.25f;
    [SerializeField] private float m_PositionOffset = 1.25f;
    [SerializeField] private bool m_DisableLayout = false;
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
        if (m_DisableLayout)
        {
            m_ParentCanvas.overrideSorting = true;
            m_ParentCanvas.sortingOrder = 100;
            m_MyLayout.enabled = false;
            
        }
        m_RectTransform.localScale = m_OriginalScale * m_HoverScale;
        m_RectTransform.localPosition = new Vector2(m_OriginalPosition.x, m_OriginalPosition.y + m_PositionOffset);
    }


    // Called when the mouse exits the image
    public void OnPointerExit(PointerEventData eventData)
    {
        m_IsHovering = false;
        // Reset the scale of the Image
        if (m_DisableLayout)
        {
            m_ParentCanvas.overrideSorting = false;
            m_ParentCanvas.sortingOrder = 0;
            m_MyLayout.enabled = true;
            
        }
        m_RectTransform.localScale = m_OriginalScale;
        m_RectTransform.localPosition = m_OriginalPosition;
    }
}