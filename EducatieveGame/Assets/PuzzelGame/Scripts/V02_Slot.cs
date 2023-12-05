using UnityEngine;
using UnityEngine.EventSystems;

public class V02_Slot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            if (dropped.GetComponent<V02_Piece>() != null)
            {
                V02_Piece piece = dropped.GetComponent<V02_Piece>();
                piece.ParentAfterDrag = transform;
            }
        }
    }
}
