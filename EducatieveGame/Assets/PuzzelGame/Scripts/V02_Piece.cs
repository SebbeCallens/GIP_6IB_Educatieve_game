using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class V02_Piece : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image _img;
    private Transform _parentAfterDrag;
    private TextMeshProUGUI _currentCoords;
    public Image Img { get { return _img; } }
    public Transform ParentAfterDrag { get { return _parentAfterDrag; } set { _parentAfterDrag = value; } }
    public TextMeshProUGUI CurrentCoords { get { return _currentCoords; } set { _currentCoords = value; } }

    void Start()
    {
        CurrentCoords = GameObject.FindGameObjectWithTag("TMP").GetComponent<TextMeshProUGUI>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Img.raycastTarget = false;
        CurrentCoords.text = this.name;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(ParentAfterDrag);
        Img.raycastTarget = true;
        transform.position = new(transform.parent.position.x, transform.parent.position.y, 0);
        CurrentCoords.text = "";
    }
}
