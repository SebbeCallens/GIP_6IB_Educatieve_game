using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FigureButton : MonoBehaviour
{
    private GameObject _menuLogObject;
    private MenuLogic _menuLog;

    private GameObject MenuLogObject { get => _menuLogObject; set => _menuLogObject = value; }
    private MenuLogic MenuLog { get => _menuLog; set => _menuLog = value; }

    private void Awake()
    {
        MenuLogObject = GameObject.Find("MenuLogic");
        MenuLog = MenuLogObject.GetComponent<MenuLogic>();
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
        PlayerPrefs.SetString("figure", figureName);
        Image[] difficultyImages = GetComponentsInChildren<Image>();
        int difficulty = Mathf.RoundToInt(difficultyImages[1].fillAmount * 5f);
        if (difficulty == 0)
        {
            difficulty = 1;
        }
        PlayerPrefs.SetInt("difficulty", difficulty);
        MenuLog.Open(1);
    }
}
