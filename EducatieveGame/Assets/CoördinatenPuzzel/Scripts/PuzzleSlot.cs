using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleSlot : MonoBehaviour, IDropHandler
{
    private static bool _dropping;

    private static bool Dropping { get => _dropping; set => _dropping = value; }

    public void OnDrop(PointerEventData eventData) //puzzelstuk ontvangen
    {
        if (!Dropping)
        {
            Dropping = true;
            if (transform.childCount == 0)
            {
                GameObject dropped = eventData.pointerDrag;
                if (dropped.GetComponent<PuzzlePiece>() != null)
                {
                    PuzzlePiece piece = dropped.GetComponent<PuzzlePiece>();
                    piece.ParentAfterDrag = transform;
                }
            }
            Dropping = false;
        }
    }
}
