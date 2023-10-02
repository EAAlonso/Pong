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
        _rb = GetComponent<Rigidbody2D>(); // Le asigno a _rb el componente Rigidbody2D del objeto invocador.
        _transf = GetComponent<Transform>(); // Le asigno a _transf el componente Transform del objeto invocador.

        _gameController = GameObject.FindGameObjectWithTag("Control"); // Busco el objeto con etiqueta Control y lo 
                                                                       // asigno al objeto _gameController.

        _player1 = GameObject.Find("Player1"); // Asocio al jugador 1 al objeto _player1
        _player2 = GameObject.Find("Player2"); // Asocio al jugador 2 al objeto _player2

        _speed = 4f; // Velocidad de la pelota
        _direction = Vector2.one.normalized; // La direccion que empieza la pelota (1,1)
    }

    private void Update()
    {
        if(_redEnabled) // Verifica si el boost rojo esta activo.
        {
            RedEffect(); // Llamado a la función que aplica el efecto rojo. 

            _timeLeftRED -= Time.deltaTime; // Implementacion de un timer para el boost rojo.
            if (_timeLeftRED < 0) // Esto va a suceder cuando el tiempo exceda el maximo.
            {
                _redEnabled = false;

                SpriteRenderer sprite = GetComponent<SpriteRenderer>(); // Asigno a sprite el componente de 
                                                                        // SpriteRenderer del objeto invocador.
                sprite.color = new Color(1f, 1f, 1f, 1f); // Vuelvo a la normalidad la pelota.
            }
        }

        if (_cyanEnabledP1) // Verifica si el boost cian esta activo para el player 1.
        {
            CyanEffect(_player1); // Llamado a la funcion que aplica el efecto cian. 

            _timeLeftCyanP1 -= Time.deltaTime; // Implementacion del timer para el boost cian.
            if (_timeLeftCyanP1 < 0)
            {
                _cyanEnabledP1 = false;
                CyanEffectRemove(_player1); // Llamado a la funcion que remueve este efecto.
            }
        }

        if (_cyanEnabledP2) // Verifica si el boost cian esta activo para el player 2.
        {
            CyanEffect(_player2);

            _timeLeftCyanP2 -= Time.deltaTime;
            if (_timeLeftCyanP2 < 0)
            {
                _cyanEnabledP2 = false;
                CyanEffectRemove(_player2);
            }
        }

        if (_blackEnabledP1) // Verifica si el boost negro esta activo para el player 1.
        {
            BlackEffect(_player1); // Llamado a funcion que aplica el efecto negro.

            _timeLeftBlackP1 -= Time.deltaTime; // Implementacion del timer para el boost negro.
            if(_timeLeftBlackP1 < 0)
            {
                _blackEnabledP1 = false;
                BlackEffectRemove(_player1); // Esta funcion remueve el efecto negro.
            }
        }

        if (_blackEnabledP2) // Verifica si el boost negro esta activo para el player 2.
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
        _rb.velocity = _direction * _speed; // Le cambia el vector velocidad al RigidBody segun la direccion 
                                            // y la velocidad.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Colision con Pared
        {
            _direction.y = -_direction.y; // Cambio de direccion en la ordenada.
        }
        else if (collision.gameObject.CompareTag("Player")) // Colision con Jugador
        {
            _direction.x = -_direction.x; // Cambio de direccion en la abscisa.
            _speed += 2;
        }
        else if (collision.gameObject.CompareTag("Point")) // Colision punto para jugador
        {
            if(_direction.x > 0) // Punto para player 1
            {
                _gameController.GetComponent<Scores>().ScorePlayer("Player1"); // Llamada al metodo ScorePlayer de 
                                                                               // la clase Scores.
            }
            else // Punto para player 2
            {
                _gameController.GetComponent<Scores>().ScorePlayer("Player2");
            }

            // Reseteamos la pelota
            ResetBall();
        }
        // Colision con los boosts
        else if (collision.gameObject.CompareTag("Boost"))
        {
            // Colision con boost azul
            if(collision.gameObject.name == "boost_blue")
            {
                BlueEffect(); // Llamada a la funcion del efecto azul.
            }
            // Colision con boost rojo
            else if(collision.gameObject.name == "boost_red")
            {
                // Activa el timer para el efecto rojo
                _redEnabled = true;
                _timeLeftRED = _timeMaxRED;
            }
            // Colision con boost cian
            else if(collision.gameObject.name == "boost_cyan")
            {
                // Segun la direccion que tenga la pelota, va a activar el timer del efecto.
                // Es un beneficio, por ejemplo: si el jugador 1 le pega a la pelota y esta toca el cuadrado cian
                // el "paddle" del jugador 1 va a agrandar su tamaño.
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
                // Segun la direccion que tenga la pelota, se activara el timer del efecto negro.
                // Perjuicio para el jugador contrario, si el jugador 1 le pega con la pelota a uno de estos cuadrados,
                // el jugador 2 va a tener el tamanio disminuido.
                if (_direction.x > 0) // Caso achica el paddle del player 1.
                {
                    _blackEnabledP2 = true;
                    _timeLeftBlackP2 = _timeMaxBlack;
                }
                else // Caso achica el paddle del player 2.
                {
                    _blackEnabledP1 = true;
                    _timeLeftBlackP1 = _timeMaxBlack;
                }
            }
            else if(collision.gameObject.name == "boost_yellow")
            {
                if (_direction.x > 0) // Caso player 1 consigue 30 de energia.
                {
                    _player1.GetComponent<Energy>().YellowBoost(); // Llamado del metodo YellowBoost de la clase Energy
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
        _direction.x = -_direction.x; // Rebota la pelota en el boost.
        _speed++; // Pequenio aumento de velocidad de la pelota.
    }
    private void RedEffect()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1f, 1f, 1f, 0.1f); // Le baja la transparencia a la pelota un 90%.        
    }

    private void CyanEffect(GameObject player)
    {
        player.transform.localScale = new Vector3(1, 2, 1); // Aumento del tamanio vertical del jugador. 
    }

    private void CyanEffectRemove(GameObject player)
    {
        player.transform.localScale = new Vector3(1, 1, 1); // Vuelve al tamanio normal al jugador.
    }

    private void BlackEffect(GameObject player)
    {
        player.transform.localScale = new Vector3(1, 0.5f, 1); // Disminuye el tamanio vertical del jugador.

        player.GetComponent<PlayerMovement>().SetSpeed(8f); // Llamado al metodo SetSpeed de la clase PlayerMovement
                                                            // para aumentar su velocidad el doble.
    }

    private void BlackEffectRemove(GameObject player)
    {
        player.transform.localScale = new Vector3(1, 1, 1); // Vuelve el tamanio normal al jugador

        player.GetComponent<PlayerMovement>().SetSpeed(4f); // Llamado al metodo SetSpeed de la clase PlayerMovement 
                                                            // para volver su velocidad a su valor normal.
    }

    // Funcion que devuelve el valor de la bandera del boost negro segun que jugador se pase por parametro.
    public bool BlackEnabled(string player)
    {
        if(player == "Player1")
            return _blackEnabledP1;

        return _blackEnabledP2; 
    }

    // Reseteo de la pelota
    public void ResetBall()
    {
        GameObject ballInstances;

        _transf.position = Vector2.zero; // Posicion (0,0) de la pelota
        _direction.x = -_direction.x;
        _speed = 4f;

        _player1.GetComponent<Energy>().RechargeEnergy(); // Llamado a RechargeEnergy de la clase Energy
        _player2.GetComponent<Energy>().RechargeEnergy(); // Le asignamos 100 de energia a los dos jugadores.

        if (_redEnabled) // Si esta activo el boost rojo, lo desactiva.
        {
            _timeLeftRED = -1;
        }

        if(_cyanEnabledP1 || _cyanEnabledP2) // Si esta activo alguno de los dos boost cyan, los desactiva.
        {
            _timeLeftCyanP1 = -1;
            _timeLeftCyanP2 = -1;
        }

        if (_blackEnabledP1 || _blackEnabledP2) // Si esta activo alguno de los boost negros, los desactiva.
        {
            _timeLeftBlackP1 = -1;
            _timeLeftBlackP2 = -1;
        }

        ballInstances = GameObject.FindGameObjectWithTag("Control"); // El objeto local ballInstances se vincula con el
                                                                     // objeto con la etiqueta Control

        while (ballInstances.GetComponent<InstantiationBoost>().GetBoostInGame()
            != 0) // Getter de la cantidad de boosts instanciados en juego.
        {
            // Limpieza de TODOS los boost del campo de juego.
            ballInstances.GetComponent<InstantiationBoost>().LimpiarBoost();
        }
    }
}
