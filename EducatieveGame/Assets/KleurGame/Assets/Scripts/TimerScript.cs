using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{

    private float targetTime = 10.0f;
    private bool _timerEnded;

    [SerializeField] GameObject _timer;
    //[SerializeField] GameObject _scriptManager;
    

    // Start is called before the first frame update
    void Start()
    {
        _timerEnded = false;
    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;
        _timer.GetComponent<Text>().text = "tijd: " + Mathf.Round(targetTime).ToString();

        if (targetTime <= 0.0f)
        {
            timerEnded();
        }
    }

    public void timerEnded()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _timerEnded = true;
    }

    public bool GetTimerEnded()
    {
        return _timerEnded;
    }
}