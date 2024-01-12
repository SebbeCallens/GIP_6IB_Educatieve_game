using UnityEngine;
using UnityEngine.UI;

public class PuzzleButton : MonoBehaviour
{
    private static Sprite puzzleImage;

    public void CreatePuzzle(Sprite image)
    {
        Image thumbnail = transform.GetChild(0).GetComponent<Image>();
        thumbnail.sprite = image;
        thumbnail.preserveAspect = true;

        puzzleImage = image;
    }

    public void StartPuzzle()
    {
        PuzzleMenu.PuzzleImage = puzzleImage;
    }
}
