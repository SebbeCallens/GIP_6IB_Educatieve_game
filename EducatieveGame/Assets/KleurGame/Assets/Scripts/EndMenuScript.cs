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

    // Start is called before the first frame update
    void Start()
    {
        _pointsTextObject.text = "Je hebt " + MailChecker.GetPoints().ToString() + " punten verzameld!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene("Startscherm");
    }
}
