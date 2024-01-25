using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGameScript : MonoBehaviour
{
    [SerializeField] private GameObject _conformationUI;

    // Start is called before the first frame update
    void Start()
    {
        _conformationUI.SetActive(false);
    }

    public void OpenConformationUI()
    {
        _conformationUI.SetActive(true);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void Cancel()
    {
        _conformationUI.SetActive(false);
    }
}
