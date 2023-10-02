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
            _movement.y = Input.GetAxisRaw("Vertical"); // Controles del Jugador 1 setteados en las config de Unity 
        }
        else
        {
            _movement.y = Input.GetAxisRaw("Vertical2"); // Controles del Jugador 2 setteados en las config de Unity 
        }
    }

    private void FixedUpdate()
    {
        if(_movementEnabled)
        {
            // Control del movimiento con el RigidBody
            _rb.MovePosition(_rb.position + _movement * _moveSpeed * Time.fixedDeltaTime); 
        }
    }

    public void SetSpeed(float speed) // Setter de velocidad
    {
        _moveSpeed = speed;
    }

    public void SetMovement(bool switchMovement) // Flag para activar o desactivar el movimiento (lo uso en la UI)
    {
        _movementEnabled = switchMovement;
    }
}
