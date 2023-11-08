using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{

    private float targetTime = 60.0f;

    [SerializeField] GameObject _timer;
    //[SerializeField] GameObject _gameScriptManager;
    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;
        _timer.GetComponent<Text>().text = "tijd: " + Mathf.Round(targetTime).ToString();

        if (targetTime <= 0.0f)
        {
            TimerEnded();
        }
    }

    public void TimerEnded()
    {
        //aantal punten op het einde van het spel gelijk zetten aan een statische variabele in SavingVariables script. WIP
        //SavingVariables._totalPoints = this.gameObject.GetComponent<SpawnMailScript>().GetPoints();

        SceneManager.LoadScene("Eindscherm");
    }
}