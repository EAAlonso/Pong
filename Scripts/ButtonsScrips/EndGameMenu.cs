using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    public GameObject _ball;
    public GameObject _scores;

    private void Start()
    {
        _ball = GameObject.FindGameObjectWithTag("Ball");
        _scores = GameObject.FindGameObjectWithTag("Control");
    }
    public void TryAgain() // Jugar de nuevo
    {
        _ball.GetComponent<BallMovement>().ResetBall(); // Resetea la pelota
        _scores.GetComponent<Scores>().ResetGame(); // Resetea los puntajes
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Cambio de escena al menu principal
    }
}
