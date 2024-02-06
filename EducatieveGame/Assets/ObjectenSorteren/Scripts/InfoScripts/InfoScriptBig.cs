using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoScriptBig : MonoBehaviour
{
    [SerializeField] private GameObject _infoUI;
    [SerializeField] private GameObject _infoButton;
    [SerializeField] private GameObject _tabCounter;

    [SerializeField] private GameObject[] _infoTabs;        //infotabs zijn lege gameobjecten, geen panelen!

    private int _currentActiveTabIndex;

    // Start is called before the first frame update
    void Start()
    {
        //startinstellingen klaarzetten
        _infoUI.SetActive(false);
        _infoButton.SetActive(true);

        foreach (GameObject tab in _infoTabs)
        {
            tab.SetActive(false);
        }

        //het klaarzetten van de eerste tab
        _infoTabs[0].SetActive(true);
    }

    public void ToggleInfoMenu()
    {
        _infoButton.SetActive(!_infoButton.activeSelf);
        _infoUI.SetActive(!_infoUI.activeSelf);
    }

    //updatet de counter
    private void UpdateTabCounter(int value)
    {
        _tabCounter.GetComponent<TextMeshProUGUI>().text = value.ToString();
    }

    //het animeren van 2 pagina's (links en rechts)
    //parameter1 voor inkomende pagina, parameter2 voor verlatende pagina, parameter3 voor richting van animatie
    private void TweenTabs(int enterIndex, int exitIndex, char direction) // F = forwards, B = backwards
    {
        if (direction == 'F')
        {
            //verplaatsen inkomende tab (van links naar recht)
            _infoTabs[enterIndex].transform.localPosition = new Vector3(2000, _infoTabs[enterIndex].transform.localPosition.y, _infoTabs[enterIndex].transform.localPosition.z);
            LeanTween.moveLocalX(_infoTabs[enterIndex], 0f, 1);

            //verplaatsen weggaande tab (van links naar rechts)
            _infoTabs[exitIndex].transform.localPosition = new Vector3(0, _infoTabs[exitIndex].transform.localPosition.y, _infoTabs[exitIndex].transform.localPosition.z);
            LeanTween.moveLocalX(_infoTabs[exitIndex], -2000f, 1);
        }
        else
        {
            //verplaatsen inkomende tab (van rechts naar links)
            _infoTabs[enterIndex].transform.localPosition = new Vector3(-2000, _infoTabs[enterIndex].transform.localPosition.y, _infoTabs[enterIndex].transform.localPosition.z);
            LeanTween.moveLocalX(_infoTabs[enterIndex], 0f, 1);

            //verplaatsen weggaande tab (van rechts naar links)
            _infoTabs[exitIndex].transform.localPosition = new Vector3(0, _infoTabs[exitIndex].transform.localPosition.y, _infoTabs[exitIndex].transform.localPosition.z);
            LeanTween.moveLocalX(_infoTabs[exitIndex], 2000f, 1);
        }
    }

    //code voor de knop naar rechts
    public void ButtonForwardsClicked()
    {
        int _exitingTabIndex;

        Debug.Log("old active index: " + _currentActiveTabIndex);

        //de actieve pagina-indexwaarde updaten
        if (_currentActiveTabIndex + 1 < _infoTabs.Length)
        {
            _currentActiveTabIndex += 1;
        }
        else
        {
            _currentActiveTabIndex = 0;
        }

        Debug.Log("new active index: " + _currentActiveTabIndex);

        //het uitzetten van alle tabs niet nodig voor de animaties
        for (int i = 0; i < _infoTabs.Length; i++)
        {
            _infoTabs[i].SetActive(false);
        }

        //de actieve tab wordt aangezet
        _infoTabs[_currentActiveTabIndex].SetActive(true);

        //de vorige tab wordt aangezet (checken welke waarde voorop was zonder errors)
        if (_currentActiveTabIndex - 1 == -1)
        {
            _infoTabs[_infoTabs.Length - 1].SetActive(true);
            _exitingTabIndex = _infoTabs.Length - 1;
        }
        else
        {
            _infoTabs[_currentActiveTabIndex - 1].SetActive(true);
            _exitingTabIndex = _currentActiveTabIndex - 1;
        }

        Debug.Log("exiting active index: " + _exitingTabIndex);

        TweenTabs(_currentActiveTabIndex, _exitingTabIndex, 'F');
        UpdateTabCounter(_currentActiveTabIndex + 1);
    }

    //code voor de knop naar links
    public void ButtonBackwardsClicked()
    {
        int _exitingTabIndex;

        Debug.Log("old active index: " + _currentActiveTabIndex);

        if (_currentActiveTabIndex - 1 >= 0)
        {
            _currentActiveTabIndex -= 1;
        }
        else
        {
            _currentActiveTabIndex = _infoTabs.Length - 1;
        }

        Debug.Log("new active index: " + _currentActiveTabIndex);

        //het uitzetten van alle tabs niet nodig voor de animaties
        for (int i = 0; i < _infoTabs.Length; i++)
        {
            _infoTabs[i].SetActive(false);
        }

        //de actieve tab wordt aangezet
        _infoTabs[_currentActiveTabIndex].SetActive(true);

        //de volgende tab wordt aangezet (checken welke waarde voorop was zonder errors)
        if (_currentActiveTabIndex + 1 == _infoTabs.Length)
        {
            _infoTabs[0].SetActive(true);
            _exitingTabIndex = 0;
        }
        else
        {
            _infoTabs[_currentActiveTabIndex + 1].SetActive(true);
            _exitingTabIndex = _currentActiveTabIndex + 1;
        }

        Debug.Log("exiting active index: " + _exitingTabIndex);

        TweenTabs(_currentActiveTabIndex, _exitingTabIndex, 'B');
        UpdateTabCounter(_currentActiveTabIndex + 1);
    }
}
