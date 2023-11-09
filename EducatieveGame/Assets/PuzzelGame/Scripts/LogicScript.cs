using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public GameObject _gridManager;
    public GameObject _confirmMenu;
    public GameObject _scoreMenu;
    private GameObject _menu;

    private void Start()
    {
        _menu = GameObject.FindGameObjectWithTag("MenuScript");
    }

    public void MoveMenus()
    {
        _confirmMenu.transform.SetSiblingIndex(5);
        _scoreMenu.transform.SetSiblingIndex(6);
    }

    public void Klaar()
    {
        _scoreMenu.SetActive(true);
        _scoreMenu.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = _gridManager.GetComponent<GridManager2>().ReturnScore();
    }

    public void Terug()
    {
        _confirmMenu.SetActive(true);
    }

    public void Nee()
    {
        _confirmMenu.SetActive(false);
    }

    public void Ja()
    {
        _scoreMenu.SetActive(false);
        Nee();
        Destroy(_menu);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
