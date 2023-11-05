using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public GameObject _confirmMenu;

    public void Terug()
    {
        _confirmMenu.SetActive(true);
    }

    public void Nee()
    {
        _confirmMenu.SetActive(false);
    }

    public void Ja()
    {
        Nee();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
