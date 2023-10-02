using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationBoost : MonoBehaviour
{
    public GameObject boostPrefab;

    [SerializeField] private int _cantMaxBoost = 3; // Cantidad maxima de boosteos en el mapa
    public int _cantBoostInGame = 0;

    private float _timeMax = 3f; // Tiempo max para que aparezca un nuevo boost
    private float _timeLeft;

    public bool _instantiationEnabled;

    private void Start()
    {
        _timeLeft = _timeMax;
        _instantiationEnabled = true;
    } 

    void Update()
    {
        // Si la cantidad de boost en juego no supera a la cantidad maxima, y la bandera este activa, se va a activar
        // la instanciacion de objetos del tipo boost.
        if(_cantBoostInGame < _cantMaxBoost && _instantiationEnabled) 
        {
            _timeLeft -= Time.deltaTime;
            if(_timeLeft < 0)
            {
                GameObject boost;
                boost = Instantiate(boostPrefab, new Vector3(Random.Range(-7, 7), 
                        Random.Range(-3.5f, 3.5f), 0), Quaternion.identity); // Instancia los boost dentro de un rango
                                                                             // aleatorio de -7 a 7 (flotante).
                boost.name = "boost";
                _cantBoostInGame++;
                _timeLeft = _timeMax;
            }
        }
    }

    public void SetInstantiation(bool instantiationSwitch) // Seteo la bandera de activacion de instancias 
                                                           // (lo uso en el menu)
    {
        _instantiationEnabled = instantiationSwitch;
    }
    public int GetBoostInGame() // Getter de cantidad de boost en el campo de juego.
    {
        return _cantBoostInGame;
    }
    public void BoostConsumido() // Se consume un boost y lo resta del atributo _cantBoostInGame.
    {
        _cantBoostInGame--;
    }

    public void LimpiarBoost()
    {
        GameObject[] deleteBoost = GameObject.FindGameObjectsWithTag("Boost"); // Crea una lista de todos los objetos
                                                                               // que tengan como etiqueta Boost.

        if (deleteBoost != null)
        {
            int cantBoosts = deleteBoost.Length;
            for(int i = 0; i < cantBoosts; i++)
            {
                deleteBoost[i].GetComponent<Boost>().DestructorBoost(); // Llamado al metodo dtor de la clase Boost
                _cantBoostInGame--;
            }
        }
    }
}
