using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Dragging : MonoBehaviour
{
    private bool _dragging = false;
    private Vector3 _offset;

    //private int _xScreenSize = 17;
    //private int _yScreenSize = 10;

    // Update is called once per frame
    void Update()
    {
        //if the object is being dragged, change its position to the mouse's cursor + the offset from the center of the object
        //checkt als het object out of bounds is
        //zo nee, zet de positie van het object gelijk aan de muispositie
        //zo ja, de positie van het object wordt verplaatst naar zijn originele positie (alleen maar als de speler heeft gestopt met het sleepen van het object)
        if (_dragging)
        {
            if (transform.position.x < 9 && transform.position.x > -9 && transform.position.y < 5 && transform.position.y > -5)
            {
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
            }
            else
            {
                _dragging = false;
                transform.position = transform.gameObject.GetComponent<MailScript>().GetoriginalPosition();
            }
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
