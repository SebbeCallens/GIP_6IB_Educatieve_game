using UnityEngine;
using UnityEngine.UI;

public class PuzzleButton : MonoBehaviour
{
    public void CreatePuzzle(Sprite image) //puzzel knop afbeelding instellen
    {
        Image thumbnail = transform.GetChild(0).GetComponent<Image>();
        thumbnail.sprite = image;
        thumbnail.preserveAspect = true;
    }

    public void StartPuzzle() //puzzel starten
    {
        PuzzleMenuLogic.PuzzleImage = transform.GetChild(0).GetComponent<Image>().sprite;
        PuzzleMenuLogic.PuzzleName = name;
        GameObject.FindWithTag("MenuLogic").GetComponent<PuzzleMenuLogic>().OpenDifficultySelection();
    }
}
