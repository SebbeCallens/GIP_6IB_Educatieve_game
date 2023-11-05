using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzelMenu : MonoBehaviour
{
    public GameObject _menu;
    public GameObject _imgChooser;

    public void StartGame()
    {
        _menu.SetActive(false);
        _imgChooser.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Back()
    {
        _menu.SetActive(true);
        _imgChooser.SetActive(false);
    }
}
