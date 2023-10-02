using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scores : MonoBehaviour
{
    public TMP_Text _scorePlayer1;
    public TMP_Text _scorePlayer2;

    public TMP_Text _winScreen;
    public GameObject _screenTransparency;

    public GameObject _player1;
    public GameObject _player2;

    public GameObject _ball;
    public GameObject _endGameMenu;

    // Update is called once per frame
    private void Start()
    {
        _scorePlayer1.SetText("0");
        _scorePlayer2.SetText("0");

        _ball = GameObject.FindGameObjectWithTag("Ball");
        _endGameMenu.SetActive(false);

        _winScreen.enabled = false;
        _screenTransparency.SetActive(false);
    }
    void Update()
    {

    }

    public void ScorePlayer(string playerName)
    {
        if(playerName == "Player1")
        {
            int.TryParse(_scorePlayer1.text, out int score);
            score++;
            _scorePlayer1.SetText(score.ToString());

            if(score == 5)
            {
                playerWin("Jugador 1");
            }
        }
        else
        {
            int.TryParse(_scorePlayer2.text, out int score);
            score++;
            _scorePlayer2.SetText(score.ToString());

            if (score == 5)
            {
                playerWin("Jugador 2");
            }
        }
    }

    private void playerWin(string playerName)
    {
        _winScreen.SetText(playerName + " Gana!");
        _winScreen.enabled = true;
        _screenTransparency.SetActive(true);

        _player1.GetComponent<Energy>().SetEnergy(false);
        _player2.GetComponent<Energy>().SetEnergy(false);

        _player1.GetComponent<PlayerMovement>().SetMovement(false);
        _player2.GetComponent<PlayerMovement>().SetMovement(false);

        this.GetComponent<InstantiationBoost>().SetInstantiation(false);

        _endGameMenu.SetActive(true);
    }

    public void ResetGame()
    {
        _winScreen.enabled = false;
        _screenTransparency.SetActive(false);

        _scorePlayer1.SetText("0");
        _scorePlayer2.SetText("0");

        _player1.GetComponent<Energy>().SetEnergy(true);
        _player2.GetComponent<Energy>().SetEnergy(true);

        _player1.GetComponent<PlayerMovement>().SetMovement(true);
        _player2.GetComponent<PlayerMovement>().SetMovement(true);

        this.GetComponent<InstantiationBoost>().SetInstantiation(true);

        _endGameMenu.SetActive(false);
    }
}
