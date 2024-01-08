using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPickScript : MonoBehaviour
{
    public UnityEvent<Color> ColorPickerEvent;

    [SerializeField] private Texture2D _colorChart;
    [SerializeField] private GameObject _chart;

    [SerializeField] private RectTransform _cursor;
    [SerializeField] private Image _button;
    [SerializeField] private Image _cursorColor;
    
    public void PickColor(BaseEventData data)
    {
        PointerEventData pointer = data as PointerEventData;

        _cursor.position = pointer.position;

        Color pickedColor = _colorChart.GetPixel((int) (_cursor.localPosition.x * (_colorChart.height / transform.GetChild(0).GetComponent<RectTransform>().rect.height)), (int)(_cursor.localPosition.y * (_colorChart.width / transform.GetChild(0).GetComponent<RectTransform>().rect.width)));

        Debug.Log("Gekozen kleur: " + pickedColor);

        _button.color = pickedColor;
        _cursorColor.color = pickedColor;
        ColorPickerEvent.Invoke(pickedColor);
    }
}
