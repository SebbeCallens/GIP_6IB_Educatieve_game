using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenuScript : MonoBehaviour
{
    [SerializeField] GameObject _pointsTextObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SavingVariables._timerEnded)
        {
            _pointsTextObject.GetComponent<TextMeshPro>().text = SavingVariables._totalPoints.ToString();
        }
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
