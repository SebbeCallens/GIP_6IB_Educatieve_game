using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FigureButton : MonoBehaviour
{
    private FigureMenuLogic _menuLog; //de menu logica

    private FigureMenuLogic MenuLog { get => _menuLog; set => _menuLog = value; }

    private void Awake() //menu logica instellen
    {
        MenuLog = GameObject.FindWithTag("MenuLogic").GetComponent<FigureMenuLogic>();
    }

    public void LoadFigure(bool original) //laad de figuur en open het confirmatie menu
    {
        if (original)
        {
            PlayerPrefs.SetInt("original", 1);
        }
        else
        {
            PlayerPrefs.SetInt("original", 0);
        }

        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        string figureName = buttonText.text;
        FigureMenuLogic.SetFigure(figureName);
        Image[] difficultyImages = GetComponentsInChildren<Image>();
        int difficulty = Mathf.RoundToInt(difficultyImages[1].fillAmount * 5f);
        if (difficulty == 0)
        {
            difficulty = 1;
        }
        MenuLogic.SetDifficulty(difficulty);
        MenuLog.OpenMenu(1);
    }
}
