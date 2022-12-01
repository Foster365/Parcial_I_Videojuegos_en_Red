using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour // TODO # Note: Este script se llama desde las escenas Win y Game_Over, luego de desconectarse de la sala, para volver al menú principal. Se llama al método LoadScene desde el button de c/ escena.
{
    public void LoadScene()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
