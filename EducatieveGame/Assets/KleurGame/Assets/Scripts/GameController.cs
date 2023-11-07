using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] public GameObject _totalPointsObject;
    
    // Start is called before the first frame update
    void Start()
    {
        //code is WIP
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
       _totalPointsObject.GetComponent<TextMeshPro>().text = "Je hebt " + SavingVariables._totalPoints.ToString() + " punten verzameld!";
    }
}
