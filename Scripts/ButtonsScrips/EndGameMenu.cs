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
    public void TryAgain()
    {
        _ball.GetComponent<BallMovement>().ResetBall();
        _scores.GetComponent<Scores>().ResetGame();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
