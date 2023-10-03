using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailScript : MonoBehaviour
{
    [SerializeField] private string _color;
    [SerializeField] private string _text;



    // Start is called before the first frame update
    void Start()
    {
        ChooseTextAndColor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ChooseTextAndColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Color groen = new Color(0, 255, 0);
        Color rood = new Color(255, 0, 0);
        Color blauw = new Color(0, 0, 255);

        Color[] possibleColors = new Color[] { rood, groen, blauw };


        Color chosenColor = possibleColors[UnityEngine.Random.Range(0, possibleColors.Length - 1)];
        Debug.Log(chosenColor);

        spriteRenderer.color = chosenColor;
    }

    public MailScript(string color, string text)
    {
        _color = color;
        _text = text;
    }

}
