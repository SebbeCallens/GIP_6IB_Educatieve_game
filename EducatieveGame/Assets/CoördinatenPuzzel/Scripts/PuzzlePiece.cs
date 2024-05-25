using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image _img; //image component
    private Transform _parentAfterDrag; //waar het puzzelstuk terecht komt na gesleept te zijn
    private TextMeshProUGUI _currentCoords; //coordinaten text

    private Image Img { get => _img; set => _img = value; }
    public Transform ParentAfterDrag { get => _parentAfterDrag; set => _parentAfterDrag = value; }
    private TextMeshProUGUI CurrentCoords { get => _currentCoords; set => _currentCoords = value; }

    void Start() //componenten instellen
    {
        CurrentCoords = GameObject.FindGameObjectWithTag("CoordinatesText").GetComponent<TextMeshProUGUI>();
    }

    public void OnBeginDrag(PointerEventData eventData) //coordinaten text instellen
    {
        CurrentCoords.GetComponent<TextMeshProUGUI>().text = name;
        ParentAfterDrag = GameObject.FindWithTag("PuzzleSlicer").transform.GetChild(0).transform;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Img.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData) //puzzelstuk instellen naar muispositie terwijl slepen
    {
        Vector3 viewportPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new(viewportPos.x, viewportPos.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData) //puzzelstuk neerplaatsen aan het einde van versleping
    {
        CurrentCoords.text = "";
        transform.SetParent(ParentAfterDrag);
        Img.raycastTarget = true;
        transform.position = new(transform.parent.position.x, transform.parent.position.y, 0);
    }

    public void EndDrag() //puzzelstuk neerplaatsen aan het einde van versleping
    {
        CurrentCoords.text = "";
        transform.SetParent(ParentAfterDrag);
        Img.raycastTarget = true;
        transform.position = new(transform.parent.position.x, transform.parent.position.y, 0);
    }
}
