using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            if (dropped.GetComponent<Piece>() != null)
            {
                Piece piece = dropped.GetComponent<Piece>();
                piece.ParentAfterDrag = transform;
            }
        }
    }
}
