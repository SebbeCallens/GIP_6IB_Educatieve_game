using TMPro;
using UnityEngine;

public class FigureMenuLogic : MenuLogic
{
    [SerializeField] private GameObject _confirmationWarning; //de warning voor in hulpmodus
    [SerializeField] private GameObject[] _menuButtons; //de menuknoppen
    private static string _figure; //de naam van de figuur

    private GameObject ConfirmationWarning { get => _confirmationWarning; set => _confirmationWarning = value; }
    private GameObject[] MenuButtons { get => _menuButtons; set => _menuButtons = value; }
    public static string Figure { get => _figure; private set => _figure = value; }

    private void Awake()
    {
        AwakeBase();
    }

    public override void OpenMenu(int index) //open gegeven menu en verberg het standaard menu
    {
        Menus[CurrentMenu].SetActive(false);
        Menus[index].SetActive(true);
        if (index == 1)
        {
            TextMeshProUGUI text = Menus[index].GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Wil je de figuur " + Figure + " tekenen?";
            if (PlayerPrefs.GetInt("figure-assist") == 1 && ConfirmationWarning != null)
            {
                ConfirmationWarning.SetActive(true);
            }
            else
            {
                ConfirmationWarning.SetActive(false);
            }
        }
        CurrentMenu = index;
    }

    public void DisableMenuButtons()
    {
        foreach (var menuButton in MenuButtons)
        {
            menuButton.SetActive(false);
        }
    }

    public static void SetFigure(string figure) //naam figuur instellen
    {
        Figure = figure;
    }
}
