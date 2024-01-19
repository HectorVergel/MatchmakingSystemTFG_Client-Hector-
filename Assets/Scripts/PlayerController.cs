using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float m_speed = 5.0f;
    private Vector2 m_moveDirection = Vector2.zero;
    private void Start()
    {

    }

    private void Update()
    {
        PlayerInputController();
    }


    private void PlayerInputController()
    {
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_moveDirection = new Vector2(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_moveDirection = new Vector2(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_moveDirection = new Vector2(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_moveDirection = new Vector2(1, 0);
        }

        Move(m_moveDirection);
    }

    private void Move(Vector2 _direction)
    {
        transform.position += (Vector3)_direction * m_speed * Time.deltaTime;
    }
}
