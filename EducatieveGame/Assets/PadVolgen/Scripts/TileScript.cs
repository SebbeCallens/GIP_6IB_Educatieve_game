using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private PlayerScript _player;

    public void Init(bool isOffset)
    {
        if(isOffset == true)
        {
            _renderer.color = _offsetColor;
        }
        if(isOffset == false)
        {
            _renderer.color = _baseColor;
        }
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    void OnMouseDown()
    {
        Vector2 tilePos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(!_player.GetIsMoving())
        {
            if ((tilePos.x < mousePos.x || tilePos.y < mousePos.y) || (tilePos.x > mousePos.x || tilePos.y > mousePos.y))
            {
                _player.MovePlayer(tilePos);
            }
        }
    }
}
