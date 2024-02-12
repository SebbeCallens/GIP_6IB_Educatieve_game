using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image _img; //iamge component
    private Transform _parentAfterDrag; //waar het puzzelstuk terecht komt na gesleept te zijn
    private TextMeshProUGUI _currentCoords; //coordinaten text
    private RectTransform _rectTf; //recttransform van dit puzzelstuk

    private Image Img { get => _img; set => _img = value; }
    public Transform ParentAfterDrag { get => _parentAfterDrag; set => _parentAfterDrag = value; }
    private TextMeshProUGUI CurrentCoords { get => _currentCoords; set => _currentCoords = value; }
    private RectTransform RectTf { get => _rectTf; set => _rectTf = value; }

    void Start() //componenten instellen
    {
        RectTf = GetComponent<RectTransform>();
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
        Vector3 viewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float adjustedY = (viewportPos.y - 1f) * Screen.height;
        RectTf.anchoredPosition = new Vector2(viewportPos.x * Screen.width, adjustedY);
    }

    public void OnEndDrag(PointerEventData eventData) //puzzelstuk neerplaatsen einde slepen
    {
        CurrentCoords.text = "";
        transform.SetParent(ParentAfterDrag);
        Img.raycastTarget = true;
        transform.position = new(transform.parent.position.x, transform.parent.position.y, 0);
    }
}
