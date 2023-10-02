using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BallMovement : MonoBehaviour
{
    public float _speed;
    Vector2 _direction;

    Rigidbody2D _rb;
    Transform _transf;

    // Controladores Boost Rojo
    private float _timeMaxRED = 3f;
    private float _timeLeftRED;
    private bool _redEnabled = false;

    // Controladores Boost Cyan
    private float _timeMaxCyan = 5f;
    private float _timeLeftCyanP1;
    private float _timeLeftCyanP2;
    private bool _cyanEnabledP1 = false;
    private bool _cyanEnabledP2 = false;

    // Controladores Boost Black
    private float _timeMaxBlack = 5f;
    private float _timeLeftBlackP1;
    private float _timeLeftBlackP2;
    private bool _blackEnabledP1 = false;
    private bool _blackEnabledP2 = false;
    
    public GameObject _player1;
    public GameObject _player2;

    public GameObject _gameController;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transf = GetComponent<Transform>();

        _gameController = GameObject.FindGameObjectWithTag("Control");

        _player1 = GameObject.Find("Player1");
        _player2 = GameObject.Find("Player2");

        _speed = 4f;
        _direction = Vector2.one.normalized;
    }

    private void Update()
    {
        if(_redEnabled) // Verifica si el boost rojo está activo.
        {
            RedEffect();
            _timeLeftRED -= Time.deltaTime;
            if (_timeLeftRED < 0) // Esto va a suceder cuando el tiempo exceda el maximo.
            {
                _redEnabled = false;

                SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                sprite.color = new Color(1f, 1f, 1f, 1f);
            }
        }

        if (_cyanEnabledP1) // Verifica si el boost cyan está activo para el player 1.
        {
            CyanEffect(_player1);
            _timeLeftCyanP1 -= Time.deltaTime;
            if (_timeLeftCyanP1 < 0)
            {
                _cyanEnabledP1 = false;
                CyanEffectRemove(_player1);
            }
        }

        if (_cyanEnabledP2) // Verifica si el boost cyan está activo para el player 2.
        {
            CyanEffect(_player2);
            _timeLeftCyanP2 -= Time.deltaTime;
            if (_timeLeftCyanP2 < 0)
            {
                _cyanEnabledP2 = false;
                CyanEffectRemove(_player2);
            }
        }

        if (_blackEnabledP1)
        {
            BlackEffect(_player1);
            _timeLeftBlackP1 -= Time.deltaTime;
            if(_timeLeftBlackP1 < 0)
            {
                _blackEnabledP1 = false;
                BlackEffectRemove(_player1);
            }
        }

        if (_blackEnabledP2)
        {
            BlackEffect(_player2);
            _timeLeftBlackP2 -= Time.deltaTime;
            if (_timeLeftBlackP2 < 0)
            {
                _blackEnabledP2 = false;
                BlackEffectRemove(_player2);
            }
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = _direction * _speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Colisión con Pared
        {
            _direction.y = -_direction.y;
        }
        else if (collision.gameObject.CompareTag("Player")) // Colisión con Jugador
        {
            _direction.x = -_direction.x;
            _speed += 2;
        }
        else if (collision.gameObject.CompareTag("Point")) // Colisión punto para jugador
        {
            if(_direction.x > 0) // Punto para player 1
            {
                _gameController.GetComponent<Scores>().ScorePlayer("Player1");
            }
            else // Punto para player 2
            {
                _gameController.GetComponent<Scores>().ScorePlayer("Player2");
            }

            // Reseteamos la pelota
            
            ResetBall();
        }
        // Colisión con los boosts
        else if (collision.gameObject.CompareTag("Boost"))
        {
            // Colisión con boost azul
            if(collision.gameObject.name == "boost_blue")
            {
                BlueEffect();
            }
            // Colisión con boost rojo
            else if(collision.gameObject.name == "boost_red")
            {
                _redEnabled = true;
                _timeLeftRED = _timeMaxRED;
            }
            else if(collision.gameObject.name == "boost_cyan")
            {
                if(_direction.x > 0) // Caso agranda el paddle del player 1.
                {
                    _cyanEnabledP1 = true;
                    _timeLeftCyanP1 = _timeMaxCyan;
                }
                else // Caso agranda el paddle del player 2.
                {
                    _cyanEnabledP2 = true;
                    _timeLeftCyanP2 = _timeMaxCyan;
                }
            }
            else if(collision.gameObject.name == "boost_black")
            {
                if (_direction.x > 0) // Caso rota 90° el paddle del player 1.
                {
                    _blackEnabledP2 = true;
                    _timeLeftBlackP2 = _timeMaxBlack;
                }
                else // Caso rota 90° el paddle del player 2.
                {
                    _blackEnabledP1 = true;
                    _timeLeftBlackP1 = _timeMaxBlack;
                }
            }
            else if(collision.gameObject.name == "boost_yellow")
            {
                if (_direction.x > 0) // Caso player 1 consigue 30 de energia.
                {
                    _player1.GetComponent<Energy>().YellowBoost();
                }
                else // Caso player 2 consigue 30 de energia.
                {
                    _player2.GetComponent<Energy>().YellowBoost();
                }
            }
        }
    }

    private void BlueEffect()
    {
        _direction.x = -_direction.x;
        _speed++;
    }
    private void RedEffect()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1f, 1f, 1f, 0.1f);        
    }

    private void CyanEffect(GameObject player)
    {
        player.transform.localScale = new Vector3(1, 2, 1);
    }

    private void CyanEffectRemove(GameObject player)
    {
        player.transform.localScale = new Vector3(1, 1, 1);
    }

    private void BlackEffect(GameObject player)
    {
        //player.transform.rotation = Quaternion.Euler(0, 0, 90);
        player.transform.localScale = new Vector3(1, 0.5f, 1);

        player.GetComponent<PlayerMovement>().SetSpeed(8f);
    }

    private void BlackEffectRemove(GameObject player)
    {
        //player.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.transform.localScale = new Vector3(1, 1, 1);

        player.GetComponent<PlayerMovement>().SetSpeed(4f);
    }

    public bool BlackEnabled(string player)
    {
        if(player == "Player1")
            return _blackEnabledP1;

        return _blackEnabledP2; 
    }

    public void ResetBall()
    {
        GameObject ballInstances;

        _transf.position = Vector2.zero;
        _direction.x = -_direction.x;
        _speed = 4f;

        _player1.GetComponent<Energy>().RechargeEnergy();
        _player2.GetComponent<Energy>().RechargeEnergy();

        if (_redEnabled)
        {
            _timeLeftRED = -1;
        }

        if(_cyanEnabledP1 || _cyanEnabledP2)
        {
            _timeLeftCyanP1 = -1;
            _timeLeftCyanP2 = -1;
        }

        if (_blackEnabledP1 || _blackEnabledP2)
        {
            _timeLeftBlackP1 = -1;
            _timeLeftBlackP2 = -1;
        }

        ballInstances = GameObject.FindGameObjectWithTag("Control"); 

        while (ballInstances.GetComponent<InstantiationBoost>().GetBoostInGame()
            != 0)
        {
            ballInstances.GetComponent<InstantiationBoost>().LimpiarBoost();
        }
    }
}
