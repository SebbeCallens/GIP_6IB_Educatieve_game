using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image _img;
    private Transform _parentAfterDrag;
    private TextMeshProUGUI _currentCoords;
    private RectTransform _rectTf;

    private Image Img { get => _img; set => _img = value; }
    public Transform ParentAfterDrag { get => _parentAfterDrag; set => _parentAfterDrag = value; }
    private TextMeshProUGUI CurrentCoords { get => _currentCoords; set => _currentCoords = value; }
    private RectTransform RectTf { get => _rectTf; set => _rectTf = value; }

    void Start()
    {
        RectTf = GetComponent<RectTransform>();
        CurrentCoords = GameObject.FindGameObjectWithTag("CoordinatesText").GetComponent<TextMeshProUGUI>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CurrentCoords.GetComponent<TextMeshProUGUI>().text = name;
        ParentAfterDrag = GameObject.FindWithTag("Slicer").transform.GetChild(0).transform;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Img.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 viewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float adjustedY = (viewportPos.y - 1f) * Screen.height;

        RectTf.anchoredPosition = new Vector2(viewportPos.x * Screen.width, adjustedY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CurrentCoords.text = "";
        transform.SetParent(ParentAfterDrag);
        Img.raycastTarget = true;
        transform.position = new(transform.parent.position.x, transform.parent.position.y, 0);
    }
}
