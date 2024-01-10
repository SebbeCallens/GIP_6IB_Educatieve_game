using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MailScript : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private string _text;

    [SerializeField] private TextMeshPro _textObject;
    
    private Vector3 _originalPosition;


    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = transform.position;
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Color GetColor()
    {
        return _color;
    }

    public string GetText()
    {
        return _text;
    }

    public Vector3 GetoriginalPosition()
    {
        return _originalPosition;
    }

    public void SetColor(Color value)
    {
        _color = value;

        _textObject.color = value;
    }

    public void SetText(string value)
    {
        _text = value;

        _textObject.text = value;
    }
}
