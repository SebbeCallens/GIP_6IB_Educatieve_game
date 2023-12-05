using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class V02_LogicScript : MonoBehaviour
{
    [SerializeField] private GameObject _confirmScreen;
    [SerializeField] private GameObject _scoreScreen;
    public GameObject ConfirmScreen {  get { return _confirmScreen; } }
    public GameObject ScoreScreen { get { return _scoreScreen; } }
    
    public void TerugKnop()
    {
        _confirmScreen.SetActive(true);
    }
    public void KlaarKnop()
    {
        _scoreScreen.SetActive(true);
    }
    public void CancelKnop()
    {
        _confirmScreen.SetActive(false);
        _scoreScreen.SetActive(false);
    }
    public void MenuKnop()
    {
        CancelKnop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
