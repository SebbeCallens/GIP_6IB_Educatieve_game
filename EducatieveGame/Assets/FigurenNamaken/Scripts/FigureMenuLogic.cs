using System.IO;
using TMPro;
using UnityEngine;

public class FigureMenuLogic : MenuLogic
{
    [SerializeField] private GameObject _confirmationWarning; //de warning voor in hulpmodus
    [SerializeField] private GameObject _deleteConfirmation; //de warning voor verwijderen figuur
    [SerializeField] private GameObject[] _menuButtons; //de menuknoppen
    [SerializeField] private GameObject _deleteButton; //de menuknoppen
    private GameObject _deletedFigureButton;
    private static string _figure; //de naam van de figuur

    private GameObject ConfirmationWarning { get => _confirmationWarning; set => _confirmationWarning = value; }
    private GameObject[] MenuButtons { get => _menuButtons; set => _menuButtons = value; }
    private GameObject DeleteButton { get => _deleteButton; set => _deleteButton = value; }
    private GameObject DeletedFigureButton { get => _deletedFigureButton; set => _deletedFigureButton = value; }
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
            TextMeshProUGUI texts = _deleteConfirmation.GetComponentInChildren<TextMeshProUGUI>();
            texts.text = "Wil je de figuur " + Figure + " verwijderen?";
            if (PlayerPrefs.GetInt("figure-assist") == 1 && ConfirmationWarning != null)
            {
                ConfirmationWarning.SetActive(true);
            }
            else
            {
                ConfirmationWarning.SetActive(false);
            }

            if (PlayerPrefs.GetInt("original") == 0)
            {
                DeleteButton.SetActive(true);
            }
            else
            {
                DeleteButton.SetActive(false);
            }
        }
        CurrentMenu = index;
    }

    public void DeleteFigure()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Figuren", Figure + ".txt");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        Destroy(DeletedFigureButton);
        CloseDeleteFigure();
        OpenMenu(0);
    }

    public void OpenDeleteFigure()
    {
        _deleteConfirmation.SetActive(true);
    }

    public void CloseDeleteFigure()
    {
        _deleteConfirmation.SetActive(false);
    }

    public void SetDeleteFigure(GameObject figure)
    {
        DeletedFigureButton = figure;
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
