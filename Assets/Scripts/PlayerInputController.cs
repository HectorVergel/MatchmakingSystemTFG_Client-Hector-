using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float m_speed = 5.0f;
    private Vector2 m_moveDirection = Vector2.zero;
    private void Start()
    {

    }

    private void Update()
    {
    }


   

    private void Move(Vector2 _direction)
    {
        transform.position += (Vector3)_direction * m_speed * Time.deltaTime;
    }
}
