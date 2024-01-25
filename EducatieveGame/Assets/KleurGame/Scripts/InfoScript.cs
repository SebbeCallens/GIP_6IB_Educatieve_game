using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScript : MonoBehaviour
{
    [SerializeField] private GameObject _infoUI;
    
    void Start()
    {
        _infoUI.SetActive(false);
    }

    public void ToggleInfoUI()
    {
        _infoUI.SetActive(!_infoUI.activeSelf);
    }
}
