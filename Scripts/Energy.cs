using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class Energy : MonoBehaviour
{
    public TMP_Text energyPlayer;

    KeyCode keyCode;
    private float _energy;
    public bool _energyEnabled;

    public GameObject progressBar;

    public GameObject _ballMovement;

    void Start()
    {
        _energyEnabled = true;
        energyPlayer.SetText("100"); // Cambia el texto de la energia del jugador asociado a 100 (inicio)
        _energy = 100f;

        _ballMovement = GameObject.FindGameObjectWithTag("Ball");

        if (this.name == "Player1") // Si el nombre del jugador es Player1, su tecla asociada a la energia va a ser
                                    // la barra espaciadora, en cambio si es el Player2 su tecla sera el control derecho.
        {
            keyCode = KeyCode.Space;
        }
        else
        {
            keyCode = KeyCode.RightControl;
        }
    }
    
    void Update()
    {
        if(Input.GetKey(keyCode) && _energy > 0 && _energyEnabled) // Si se mantiene el boton pulsado,
                                                                  // gasta energia y aumenta la
                                                                  // velocidad del jugador
        {
            this.GetComponent<PlayerMovement>().SetSpeed(16f); // Llamado del metodo SetSpeed de la clase PlayerMovement
            ConsumeEnergy(); // Funcion que consume la energia
        }

        if(Input.GetKeyUp(keyCode)) // Si se suelta la tecla, vuelve la
                                    // velocidad normal
        {
            if(this.name == "Player1" && _ballMovement.GetComponent<BallMovement>().BlackEnabled("Player1"))
            {
                this.GetComponent<PlayerMovement>().SetSpeed(8f);
            }

            if(this.name == "Player2" && _ballMovement.GetComponent<BallMovement>().BlackEnabled("Player2"))
            {
                this.GetComponent<PlayerMovement>().SetSpeed(8f);
            }

            this.GetComponent<PlayerMovement>().SetSpeed(4f);
        }

        if(_energy <= 0) // Evita que quede en 0,1 o -0,1 y le pone 0 al texto.
        {
            energyPlayer.SetText("0");
        }
    }

    private void ConsumeEnergy()
    {
        float.TryParse(energyPlayer.text, out _energy); // Parsea el texto a un float
        _energy -= Time.fixedDeltaTime * 32.5f; // Baja la energia segun el tiempo fijo entre las actualizaciones de 
                                                // fisica del juego y una constante.
        progressBar.transform.localScale = new Vector3(_energy / 100, 1, 1); // Controla la barra del progreso de la UI.
        energyPlayer.SetText(_energy.ToString("F1")); // Parse de float a string con 1 decimal y setea el texto de la 
                                                      // energia del jugador en la UI.
    }

    public void RechargeEnergy() // Devuelve el 100% de la energia
    {
        float.TryParse(energyPlayer.text, out _energy);
        _energy = 100;
        progressBar.transform.localScale = new Vector3(1, 1, 1);
        energyPlayer.SetText(_energy.ToString("F1"));
    }

    public void YellowBoost() // Esto ocurre cuando la pelota toca el boost amarillo
    {
        float.TryParse(energyPlayer.text, out _energy);
        if(_energy <= 70) // Si la energia es menos de 70, agrega 30 de energia. Si es mayor, le asigna 100 
                          // (evita desbordes de energia)
        {
            _energy += 30;
        }
        else
        {
            _energy = 100;
        }

        energyPlayer.SetText(_energy.ToString("F1"));
        progressBar.transform.localScale = new Vector3(_energy / 100, 1, 1);
    }

    public void SetEnergy(bool switchEnergy) // Flag de energia (para el menu de fin de juego)
    {
        _energyEnabled = switchEnergy;
    }
}
