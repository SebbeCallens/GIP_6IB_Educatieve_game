using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    private string _coords;

    public string Coords { get { return _coords; } set { _coords = value; } }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            Piece piece = dropped.GetComponent<Piece>();
            piece._parentAfterDrag = transform;
        }
    }
}
