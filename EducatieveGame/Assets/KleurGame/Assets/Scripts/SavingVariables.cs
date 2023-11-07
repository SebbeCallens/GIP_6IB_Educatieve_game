using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingVariables : MonoBehaviour
{
    [SerializeField] public static int _totalPoints;
    [SerializeField] public static bool _timerEnded;

    // Start is called before the first frame update
    void Start()
    {
        _timerEnded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<TimerScript>().GetTimerEnded())
        {
            _timerEnded = true;
            _totalPoints = this.gameObject.GetComponent<SpawnMailScript>().GetPoints();
        }
    }
}
