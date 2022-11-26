using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCOntroler : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AudioListener.volume = 0;
    }
    public void AboutButton()
    {
        SceneManager.LoadScene("About");
    }
    public void BackButton()
    {
        Debug.Log("back");
        SceneManager.LoadScene(0);
    }
}
