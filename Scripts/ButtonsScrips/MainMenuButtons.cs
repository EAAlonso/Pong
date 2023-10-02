using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // Cambia de escena a "SampleScene" (me olvide de cambiar el nombre de 
                                               // la escena al principio y tuve miedo de cambiarlo cuando la termine)
    }

    public void QuitGame()
    {
        Application.Quit(); // Termina el proceso.
    }
}
