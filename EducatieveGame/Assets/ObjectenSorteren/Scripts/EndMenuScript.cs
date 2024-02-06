using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenuScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _pointsTextObject;

    void Start()
    {
        _pointsTextObject.text = "Je hebt " + MailChecker.Points.ToString() + " punten verzameld!";
    }


    public void EndGame()
    {
        Application.Quit();
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene("Startscherm");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
