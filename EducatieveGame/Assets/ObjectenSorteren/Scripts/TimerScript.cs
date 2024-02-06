using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{

    private float _targetTime;

    [SerializeField] GameObject _timer;
    //[SerializeField] GameObject _gameScriptManager;
    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (SettingsDataScript._timerSetting)
        {
            _targetTime -= Time.deltaTime;
            _timer.GetComponent<Text>().text = "tijd: " + Mathf.Round(_targetTime).ToString();

            if (_targetTime <= 0.0f)
            {
                TimerEnded();
            }
        }
        else
        {
            _timer.GetComponent<Text>().text = "tijd: oneindig";
        }
    }

    private void Awake()
    {
        if (SettingsDataScript._timerSetting) 
        {
            _targetTime = SettingsDataScript._timerValue;
        }
        else
        {
            _targetTime = 99999999999999f;
        }
    }

    public void TimerEnded()
    {
        SceneManager.LoadScene("Eindscherm");
    }
}