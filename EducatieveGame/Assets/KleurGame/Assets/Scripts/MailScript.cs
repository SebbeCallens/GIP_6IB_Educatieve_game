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
        
        //ChooseTextAndColor();
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

    //moet deels herschreven worden, de mogelijke kleuren moeten komen van StatsScript.
    /*private void ChooseTextAndColor()
    {
        //color of text
        

        //Color groen = new Color(0, 255, 0);
        //Color rood = new Color(255, 0, 0);
        //Color blauw = new Color(0, 0, 255);

        //these arrays have to be keep on updated so the values in them are the same
        Color[] colors = new Color[] { rood, groen, blauw };
        List <string> possibleColors = new List<string> { "rood", "groen", "blauw" };

        string chosenColor = possibleColors[UnityEngine.Random.Range(0, possibleColors.Count)];
        
        //omgekeerd checken van elke kleur om fouten te voorkomen.
        for (int i = possibleColors.Count - 1; i >= 0; i--)
        {
            if (possibleColors[i] == chosenColor)
            {
                _textObject.color = colors[i];
                _color = colors[i];

                possibleColors.RemoveAt(i);
                break;
            }
        }

        Debug.Log("color chosen: " + chosenColor);

        //text
        _text = possibleColors[UnityEngine.Random.Range(0, possibleColors.Count)];

        _textObject.text = _text;
        Debug.Log("text chosen: " + _text);
    }*/

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
