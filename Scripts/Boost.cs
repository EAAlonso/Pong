using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Boost : MonoBehaviour
{
    private int _typeBoost; 
    SpriteRenderer _sprite;

    //public InstantiationBoost instantiationBoost;

    public GameObject _gameController;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _gameController = GameObject.FindGameObjectWithTag("Control");

        _typeBoost = Random.Range(0, 5);

        switch(_typeBoost)
        {
            case 0: // (0) Azul -> la pelota rebota como si fuera el jugador
                _sprite.color = Color.blue;
                gameObject.name = "boost_blue";
            break;
            case 1: // (1) Rojo -> la pelota se vuelve invisible por 2 sec.
                _sprite.color = Color.red;
                gameObject.name = "boost_red";
            break;
            case 2: // (2) Cyan -> agranda el paddle 
                _sprite.color = Color.cyan;
                gameObject.name = "boost_cyan";
            break;
            case 3: // (3) Black -> vuelta 90°
                _sprite.color = Color.black;
                gameObject.name = "boost_black";
            break;
            case 4: // (4) Yellow -> da 30 de energia al que lo toca.
                _sprite.color = Color.yellow;
                gameObject.name = "boost_yellow";
            break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball")) // Colisión con pelota
        {
            _gameController.GetComponent<InstantiationBoost>().BoostConsumido();
            DestructorBoost();
        }
    }

    public void DestructorBoost()
    {
        Destroy(this.gameObject);
    }
}
