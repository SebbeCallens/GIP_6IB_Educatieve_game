using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButtonScript : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private string _name;

    public Color Color
    {
        get { return _color; }
    }

    public string Name
    {
        get { return _name; }
    }
}
