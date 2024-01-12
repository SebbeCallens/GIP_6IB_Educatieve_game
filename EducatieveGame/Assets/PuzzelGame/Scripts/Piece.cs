using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Piece : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Image _img;
    private Transform _parentAfterDrag;
    private TextMeshProUGUI _currentCoords;

    private Image Img { get => _img; set => _img = value; }
    public Transform ParentAfterDrag { get => _parentAfterDrag; set => _parentAfterDrag = value; }
    private TextMeshProUGUI CurrentCoords { get => _currentCoords; set => _currentCoords = value; }

    void Start()
    {
        CurrentCoords = GameObject.FindGameObjectWithTag("CoordinatesText").GetComponent<TextMeshProUGUI>();
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
        transform.SetAsFirstSibling();
    }
}
