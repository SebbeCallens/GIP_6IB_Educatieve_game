using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;

    public void Init(bool isOffset)
    {
        if(isOffset == true)
        {
            _renderer.color = _baseColor;
        }
        if(isOffset == false)
        {
            _renderer.color = _baseColor;
        }
    }
}
