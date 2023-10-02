using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _moveSpeed = 4f;
    Vector2 _movement;

    [SerializeField] private Rigidbody2D _rb;

    public bool _movementEnabled;

    private void Start()
    {
        _movementEnabled = true;
    }

    void Update()
    {
        if(gameObject.name == "Player1")
        {
            _movement.y = Input.GetAxisRaw("Vertical"); // Jugador 1
        }
        else
        {
            _movement.y = Input.GetAxisRaw("Vertical2"); // Jugador 2
        }
    }

    private void FixedUpdate()
    {
        if(_movementEnabled)
        {
        _rb.MovePosition(_rb.position + _movement * _moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void SetSpeed(float speed)
    {
        _moveSpeed = speed;
    }

    public void SetMovement(bool switchMovement)
    {
        _movementEnabled = switchMovement;
    }
}
