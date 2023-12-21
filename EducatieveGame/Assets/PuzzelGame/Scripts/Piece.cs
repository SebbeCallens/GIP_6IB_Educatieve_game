using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Piece : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Transform _parentAfterDrag;
    private Image _imageRenderer;
    private Sprite _img;
    private RectTransform _rectTransform;
    private string _coords;

    void Start()
    {
        _imageRenderer = this.GetComponent<Image>();
        _img = _imageRenderer.sprite;
        _rectTransform = GetComponent<RectTransform>();
    }

    public Sprite Img { get { return _img; } set { _img = value; _imageRenderer.sprite = value; } }
    public string Coords { get { return _coords; } set { _coords = value; } }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _imageRenderer.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_parentAfterDrag);
        _imageRenderer.raycastTarget = true;
    }
}
