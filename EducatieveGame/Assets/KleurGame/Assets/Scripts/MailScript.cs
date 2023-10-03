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

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ChooseTextAndColor()
    {
        Color groen = new Color(0, 1, 0);
        //Color possibleColors[] = new Color[] {groen};
    }

    public MailScript(string color, string text)
    {
        _color = color;
        _text = text;
    }

}
