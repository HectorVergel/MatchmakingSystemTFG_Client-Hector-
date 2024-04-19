using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CardsHandlerBase : MonoBehaviour
{
    [SerializeField] private float m_Speed = 10.0f;
    protected virtual void MoveCard(Card _card, Vector2 _position)
    {
        StartCoroutine(MoveCoroutine(_card, _position));
    }

    private IEnumerator MoveCoroutine(Card _card, Vector2 _position)
    {
        float l_Distance = Vector2.Distance(_card.transform.position, _position);
        Vector2 l_Direction = (_position - (Vector2)_card.transform.position).normalized;
        while (l_Distance > 0.5f)
        {
            _card.transform.position += (Vector3)(l_Direction * m_Speed * Time.deltaTime);
            yield return null;
        }
       
    }

    protected void FlipCard(Card _card)
    {
        _card.Flip();
    }
    
    
}