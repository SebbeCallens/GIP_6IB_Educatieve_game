using UnityEngine;
using UnityEngine.UI;

public class PuzzleButton : MonoBehaviour
{
    public void CreatePuzzle(Sprite image)
    {
        Image thumbnail = transform.GetChild(0).GetComponent<Image>();
        thumbnail.sprite = image;
        thumbnail.preserveAspect = true;
    }

    public void StartPuzzle()
    {
        PuzzleMenu.PuzzleImage = transform.GetChild(0).GetComponent<Image>().sprite;
        GameObject.FindWithTag("PuzzleMenu").GetComponent<PuzzleMenu>().OpenDifficulty();
    }
}
