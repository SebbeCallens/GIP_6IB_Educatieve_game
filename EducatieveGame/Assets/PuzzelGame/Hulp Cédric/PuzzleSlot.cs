using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleSlot : MonoBehaviour, IDropHandler
{
    //inslotten wanneer de target cel nog geen stukje heeft heeft
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            if (dropped.GetComponent<PuzzlePiece>() != null)
            {
                PuzzlePiece piece = dropped.GetComponent<PuzzlePiece>();
                piece.ParentAfterDrag = transform;
            }
        }
    }
}
