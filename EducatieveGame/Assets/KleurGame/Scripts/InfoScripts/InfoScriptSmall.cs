using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScript : MonoBehaviour
{
    [SerializeField] private GameObject _infoUI;
    [SerializeField] private GameObject _infoButton;
    
    void Start()
    {
        //startinstellingen klaarzetten
        _infoUI.SetActive(false);
        _infoButton.SetActive(true);
    }

    //openen / sluiten van infomenu
    public void ToggleInfoMenu()
    {
        _infoButton.SetActive(!_infoButton.activeSelf);
        _infoUI.SetActive(!_infoUI.activeSelf);
    }
}
