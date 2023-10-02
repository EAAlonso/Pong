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
        energyPlayer.SetText("100");
        _energy = 100f;

        _ballMovement = GameObject.FindGameObjectWithTag("Ball");

        if (this.name == "Player1")
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
        if(Input.GetKey(keyCode) && _energy > 0 && _energyEnabled) // Si se mantiene el botón pulsado,
                                                                  // gasta energia y aumenta la
                                                                  // velocidad del jugador
        {
            this.GetComponent<PlayerMovement>().SetSpeed(16f);
            ConsumeEnergy();
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

        if(_energy <= 0) // Evita que quede en 0,1 o -0,1 y le pone 0.
        {
            energyPlayer.SetText("0");
        }
    }

    private void ConsumeEnergy()
    {
        float.TryParse(energyPlayer.text, out _energy);
        _energy -= Time.fixedDeltaTime * 4.5f;
        progressBar.transform.localScale = new Vector3(_energy / 100, 1, 1);
        energyPlayer.SetText(_energy.ToString("F1"));
    }

    public void RechargeEnergy()
    {
        float.TryParse(energyPlayer.text, out _energy);
        _energy = 100;
        progressBar.transform.localScale = new Vector3(1, 1, 1);
        energyPlayer.SetText(_energy.ToString("F1"));
    }

    public void YellowBoost()
    {
        float.TryParse(energyPlayer.text, out _energy);
        if(_energy <= 70)
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

    public void SetEnergy(bool switchEnergy)
    {
        _energyEnabled = switchEnergy;
    }
}
