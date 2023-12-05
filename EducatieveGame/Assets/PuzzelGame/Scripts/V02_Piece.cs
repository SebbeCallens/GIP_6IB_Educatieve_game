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
    [SerializeField] private GameObject _currentCoords;
    public Image Img { get { return _img; } }
    public Transform ParentAfterDrag { get { return _parentAfterDrag; } set { _parentAfterDrag = value; } }
    public GameObject CurrentCoords { get { return _currentCoords; } set { _currentCoords = value; } }

    void Start()
    {
        CurrentCoords = GameObject.FindGameObjectWithTag("TMP");
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        CurrentCoords.GetComponent<TextMeshProUGUI>().text = name;
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Img.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        CurrentCoords.GetComponent<TextMeshProUGUI>().text = "";
        transform.SetParent(ParentAfterDrag);
        Img.raycastTarget = true;
    }
}
