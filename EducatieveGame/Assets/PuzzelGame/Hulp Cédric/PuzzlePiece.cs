using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image _img; //het image component
    private Transform _parentAfterDrag; //doel parent transform na slepen

    public Transform ParentAfterDrag { get => _parentAfterDrag; set => _parentAfterDrag = value; }

    //begin slepen, oude parent transform onthouden
    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _img.raycastTarget = false;
    }

    //tijdens het slepen stukje positie gelijk stellen aan positie muis
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    //einde van het slepen parent instellen en stukje inslotten
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(ParentAfterDrag);
        _img.raycastTarget = true;
        transform.position = new(transform.parent.position.x, transform.parent.position.y, 0);
    }
}
