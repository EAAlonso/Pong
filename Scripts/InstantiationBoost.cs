using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationBoost : MonoBehaviour
{
    public GameObject boostPrefab;

    [SerializeField] private int _cantMaxBoost = 3; // Cantidad máxima de boosteos en el mapa
    public int _cantBoostInGame = 0;

    private float _timeMax = 3f; // Tiempo máx para que aparezca un nuevo boost
    private float _timeLeft;

    public bool _instantiationEnabled;

    private void Start()
    {
        _timeLeft = _timeMax;
        _instantiationEnabled = true;
    } 

    void Update()
    {
        if(_cantBoostInGame < _cantMaxBoost && _instantiationEnabled)
        {
            _timeLeft -= Time.deltaTime;
            if(_timeLeft < 0)
            {
                GameObject boost;
                boost = Instantiate(boostPrefab, new Vector3(Random.Range(-7, 7), 
                        Random.Range(-3.5f, 3.5f), 0), Quaternion.identity);
                boost.name = "boost";
                _cantBoostInGame++;
                _timeLeft = _timeMax;
            }
        }
    }

    public void SetInstantiation(bool instantiationSwitch)
    {
        _instantiationEnabled = instantiationSwitch;
    }
    public int GetBoostInGame()
    {
        return _cantBoostInGame;
    }
    public void BoostConsumido()
    {
        _cantBoostInGame--;
    }

    public void LimpiarBoost()
    {
        GameObject[] deleteBoost = GameObject.FindGameObjectsWithTag("Boost"); // FALTA CONECTAR ESTO CUANDO HACEN GOL 

        if (deleteBoost != null)
        {
            int cantBoosts = deleteBoost.Length;
            for(int i = 0; i < cantBoosts; i++)
            {
                deleteBoost[i].GetComponent<Boost>().DestructorBoost();
                _cantBoostInGame--;
            }
        }
    }
}
