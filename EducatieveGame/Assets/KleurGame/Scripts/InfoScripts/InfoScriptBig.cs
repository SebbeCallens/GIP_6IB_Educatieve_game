using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScriptBig : MonoBehaviour
{
    [SerializeField] private GameObject _infoUI;
    [SerializeField] private GameObject _infoButton;

    [SerializeField] private GameObject[] _infoTabs;        //infotabs zijn lege gameobjecten, geen panelen!

    private int _currentActiveTabIndex;


    // Start is called before the first frame update
    void Start()
    {
        _infoUI.SetActive(false);
        _infoButton.SetActive(true);

        foreach (GameObject tab in _infoTabs)
        {
            tab.SetActive(false);
        }

        //het klaarzetten van de eerste tab
        _infoTabs[0].SetActive(true);

        //de onderstaande lijn code moet verplaatst worden in button
        _infoTabs[0].GetComponent<Animator>().SetTrigger("Enter");
    }

    public void ToggleInfoMenu()
    {
        _infoButton.SetActive(!_infoButton.activeSelf);
        _infoUI.SetActive(!_infoUI.activeSelf);
    }

    //toont de gekozen tab op het infoscherm
    //WIP
    private void LoadTab(int index)
    {
        
    }

    public void ButtonForwardsClicked()
    {
        //LoadTab(_currentActiveTabIndex + 1);
        _currentActiveTabIndex += 1;

        //checken als de tab bestaat
        if (_currentActiveTabIndex >= _infoTabs.Length || _currentActiveTabIndex < 0)
        {
            return;
        }

        //het aanzetten van alle tabs die nodig zijn voor animaties
        //de vorige tab zal alleen maar active gezet worden als het bestaat
        for (int i = 0; i < _infoTabs.Length; i++)
        {
            if (_currentActiveTabIndex - 1 == i)
            {
                _infoTabs[i].SetActive(true);
            }
            else if (_currentActiveTabIndex == i)
            {
                _infoTabs[i].SetActive(true);
            }
            else
            {
                _infoTabs[i].SetActive(false);
            }
        }

        //animaties
        _infoTabs[_currentActiveTabIndex].GetComponent<Animator>().SetTrigger("Enter");

        if (_infoTabs[_currentActiveTabIndex - 1])
        {
            _infoTabs[_currentActiveTabIndex - 1].GetComponent<Animator>().SetTrigger("Exit");
        }
    }

    public void ButtonBackwardsClicked()
    {
        LoadTab(_currentActiveTabIndex - 1);
    }
}
