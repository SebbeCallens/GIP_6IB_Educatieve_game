using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class V02_LogicScript : MonoBehaviour
{
    [SerializeField] private GameObject _confirmScreen;
    [SerializeField] private GameObject _scoreScreen;
    [SerializeField] private GameObject _gridManager;
    public GameObject ConfirmScreen {  get { return _confirmScreen; } }
    public GameObject ScoreScreen { get { return _scoreScreen; } }
    public GameObject GridManger { get { return _gridManager; } }
    
    public void TerugKnop()
    {
        ConfirmScreen.SetActive(true);
    }
    public void KlaarKnop()
    {
        ScoreScreen.SetActive(true);
        int score = GridManger.GetComponent<V02_GridManager>().ReturnScore();
        int max = 0;
        foreach (GameObject slot in GridManger.GetComponent<V02_GridManager>().Slots)
        {
            max++;
        }
        ScoreScreen.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Je behaalde een " + score + "/" + max + " (" + ((double)score/(double)max)*100 + "%)";
    }
    public void SluitSchermKnop()
    {
        ConfirmScreen.SetActive(false);
        ScoreScreen.SetActive(false);
    }
    public void NaarMenuKnop()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
