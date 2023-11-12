using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragging : MonoBehaviour
{
    private bool _dragging = false;
    private Vector3 _offset;

    // Update is called once per frame
    void Update()
    {
        //if the object is being dragged, change its position to the mouse's cursor + the offset from the center of the object
        if (_dragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
        }
    }

    private void OnMouseDown()
    {
        _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _dragging = true;
    }

    private void OnMouseUp()
    {
        _dragging = false;
    }

    public void SetDragging(bool value)
    {
        _dragging = value;
    }

    public bool GetDragging()
    {
        return _dragging;
    }
}
