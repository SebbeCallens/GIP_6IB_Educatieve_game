using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Windows.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingsDataScript : MonoBehaviour
{
    //data to be transfered to the other scene
    public static bool _trashcanSetting = false;
    public static bool _conveyorSetting = false;
    public static bool _testModeSetting = false;

    public static bool _timerSetting = true;
    public static int _timerValue = 60;

    public static string _chosenDifficulty;

    public static int _pointsPerAnswer = 1;

    //geavanceerde instellingen
    public static int _rightPoints = 1;
    public static int _wrongPoints = 1;

    public static int _conveyorSpeed = 3;
    //

    public static List<GameObject> _selectedColorButtons = new List<GameObject>();
    public static List<Color> _selectedColorButtonsColors = new List<Color>();
    public static List<string> _selectedColorButtonsNames = new List<string>();

    public static List<GameObject> _colorButtons = new List<GameObject>();
    public static List<Color> _colorButtonsColors = new List<Color>();
    public static List<string> _colorButtonsNames = new List<string>();


    //referencing values
    [SerializeField] private GameObject _colorButton;
    [SerializeField] private UnityEngine.UI.Image _checkmark;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameObject[] _modeObjects = new GameObject[2]; //vuilnisbak instelling object & loopband instelling object

    private GameObject _errorTextObject;

    //other setting variables
    private GameObject _colorsSetting;
    private GameObject _settingsMenu;
    private GameObject _normalMenu;
    private GameObject _colorMenu;
    private GameObject _colorButtonsObject;

    private GameObject _timerInputFieldObject;

    private GameObject _pointsRightObject;
    private GameObject _pointsWrongObject;

    private GameObject _lastClickedColorButton;

    //private int _xValueLastButton = -125;
    //private int _yValue = 250;
    //private int _increment = 125;

    void Start()
    {
        ResetData();
        DefineMenus();
        SetColorButtonsList();
        EnableStandardColorButtons();

        //dit moet als laatste
        GameObject.Find("SettingsMenu").SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        
    }

    //zoekt alle gameobjecten nodig voor latere berekeningen
    public void DefineMenus()
    {
        _colorsSetting = GameObject.Find("ColorsSetting");
        _settingsMenu = GameObject.Find("SettingsMenu");
        _normalMenu = GameObject.Find("NormalMenu");
        _colorMenu = GameObject.Find("ColorMenu");
        _colorButtonsObject = GameObject.Find("ColorButtons");

        _timerInputFieldObject = GameObject.Find("TijdInputField");
        _pointsRightObject = GameObject.Find("PuntenJuistInstelling");
        _pointsWrongObject = GameObject.Find("PuntenFoutInstelling");

        _errorTextObject = GameObject.Find("ErrorTextObject");
    }

    //het openen/sluiten van een de instellingen
    public void SettingsButton()
    {
        if (_normalMenu.activeSelf)
        {
            _normalMenu.SetActive(false);
            _settingsMenu.SetActive(true);
        }
        else
        {
            _normalMenu.SetActive(true);
            _settingsMenu.SetActive(false);
        }
    }

    //voegt de kleurknopwaarden toe op hun juiste index
    public void SetColorButtonsList()
    {
        for (int i = 0; i < _colorMenu.transform.childCount; i++)
        {
            GameObject currentColorButton = _colorMenu.transform.GetChild(i).gameObject;
            _colorButtonsColors.Add(currentColorButton.GetComponent<ColorButtonScript>().Color);
            _colorButtonsNames.Add(currentColorButton.GetComponent<ColorButtonScript>().Name);

            _colorButtons.Add(currentColorButton);
        }
    }

    //zet rood, blauw en groen als standaard aangezette kleurwaarden
    public void EnableStandardColorButtons()
    {
        foreach (GameObject colorButton in _colorButtons)
        {
            if (colorButton.name.Equals("ColorRed") || colorButton.name.Equals("ColorGreen") || colorButton.name.Equals("ColorBlue"))
            {
                AddColorButton(colorButton);

                colorButton.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().enabled = true;
            }
        }
    }

    //zet alle variabelen terug aan hun standaard waarden
    public static void ResetData()
    {
        _trashcanSetting = false;
        _conveyorSetting = false;
        _testModeSetting = false;

        _timerSetting = true;
        _timerValue = 60;

        _pointsPerAnswer = 1;
        _rightPoints = 1;
        _wrongPoints = 1;

        _chosenDifficulty = null;

        _selectedColorButtons = new List<GameObject>();
        _selectedColorButtonsColors = new List<Color>();
        _selectedColorButtonsNames = new List<string>();

        _colorButtons = new List<GameObject>();
        _colorButtonsColors = new List<Color>();
        _colorButtonsNames = new List<string>();

        MailChecker.Points = 0;
}
    //start de kleurgame scene
    public void LoadKleurgameScene()
    {
        /*for (int i = 0; i < _selectedColorButtons.Count; i++)
        {
            Debug.Log("object: " + _selectedColorButtons[i] + "\n" + 
            "color: " + _selectedColorButtonsColors[i] + "\n" + 
            "name: " + _selectedColorButtonsNames[i]);
        }*/

        SceneManager.LoadScene("KleurGame");
    }

    public void ToggleSettingsUI()
    {
        if (_settingsMenu)
        {
            if (_settingsMenu.activeSelf)
            {
                _settingsMenu.SetActive(false);
            }
            else
            {
                _settingsMenu.SetActive(true);
            }
        }
    }

    public void ToggleTrashbinSetting(bool newValue)
    {
        _trashcanSetting = newValue;

        if (newValue)
        {
            _modeObjects[1].gameObject.GetComponent<UnityEngine.UI.Toggle>().isOn = false;
        }
    }

    public void ToggleTestModeSetting(bool newValue)
    {
        _testModeSetting = newValue;
    }

    public void ToggleConveyorSetting(bool newValue)
    {
        _conveyorSetting = newValue;

        if (newValue)
        {
            _modeObjects[0].gameObject.GetComponent<UnityEngine.UI.Toggle>().isOn = false;
        }
    }

    public void ToggleTimerSetting(bool newValue)
    {
        _timerSetting = newValue;

        if (newValue)
        {
            _timerInputFieldObject.SetActive(true);
        }
        else
        {
            _timerInputFieldObject.SetActive(false);
        }
    }

    //methode om de tijd aan te passen
    public void ChangeTime()
    {
        if (int.TryParse(_inputField.text, out int time))
        {
            if (time > 0 && time < 1000)
            {
                _timerValue = time;
            }
        }
    }

    //methode om het aantal punten bij een juist antwoord aan te passen
    public void ChangePointsRight()
    {
        if (int.TryParse(_pointsRightObject.GetComponent<TMP_InputField>().text, out int points))
        {
            if (points > 0 && points < 1000)
            {
                _rightPoints = points;
            }
        }
    }

    //methode om het aantal punten bij een fout antwoord aan te passen
    public void ChangePointsWrong()
    {
        if (int.TryParse(_pointsWrongObject.GetComponent<TMP_InputField>().text, out int points))
        {
            if (points > 0 && points < 1000)
            {
                _wrongPoints = points;
            }
        }
    }

    //tekst tonen en weer weghalen lukt nog niet.
    public bool CheckStatsValid()
    {
        bool statsValid = true;

        //ERRORS
        //checken als de gebruiker kleuren heeft gekozen
        if (_selectedColorButtons.Count == 1)
        {
            //DisplayErrorText("Er moeten minimaal 2 kleuren gekozen worden.");
            SetErrorText("Er moeten minimaal 2 kleuren gekozen worden.", Color.red);

            statsValid = false;
        }
        else if (_selectedColorButtons.Count <= 0)
        {
            //DisplayErrorText("Kies kleuren in instellingen.");
            SetErrorText("Kies kleuren in instellingen.", Color.red);

            statsValid = false;
        }
        else if (_selectedColorButtons.Count > 5)
        {
            //DisplayErrorText("Er mogen maximaal maar 5 kleuren gekozen worden.");
            SetErrorText("Er mogen maximaal maar 5 kleuren gekozen worden.", Color.red);

            statsValid = false;
        }
        //WARNINGS
        

        return statsValid;
    }

    public void SetErrorText(string text, Color color)
    {
        _errorTextObject.GetComponent<TextMeshProUGUI>().color = color;
        _errorTextObject.GetComponent<TextMeshProUGUI>().text = text;
    }

    //code voor de gemakkelijk modus
    public void EasyMode()
    {
        _chosenDifficulty = "easy";
        
        //_pointsPerAnswer = 1;
        _rightPoints = 1;
        _wrongPoints = 1;

        _timerSetting = true;
        _timerValue = 60;
        _trashcanSetting = false;

        GenerateColorButtons(3);

        LoadKleurgameScene();
    }

    //code voor de normaal modus
    public void NormalMode()
    {
        _chosenDifficulty = "normal";

        //_pointsPerAnswer = 2;
        _rightPoints = 2;
        _wrongPoints = 2;

        _timerSetting = true;
        _timerValue = 60;
        _trashcanSetting = false;

        GenerateColorButtons(4);

        LoadKleurgameScene();
    }

    //code voor de moeilijk modus
    public void DifficultMode()
    {
        _chosenDifficulty = "difficult";

        //_pointsPerAnswer = 3;
        _rightPoints = 3;
        _wrongPoints = 3;

        _timerSetting = true;
        _timerValue = 60;
        _trashcanSetting = true;

        GenerateColorButtons(5);

        LoadKleurgameScene();
    }

    //code voor de aangepaste modus
    public void ModifiedMode()
    {
        _chosenDifficulty = "modified";
        
        if (CheckStatsValid())
        {
            LoadKleurgameScene();
        }
    }


    //deze methode voegt het nieuwe 
    public void ColorMenuButtonClicked()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<UnityEngine.UI.Image>().color);

        //de knop togglet als de afbeelding active is of niet
        EventSystem.current.currentSelectedGameObject.gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().enabled = !EventSystem.current.currentSelectedGameObject.gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().enabled;

        //het toevoegen van de waarden van de knop aan de lists als het gameobject aangevinkt is. Anders, verwijder het object
        if (EventSystem.current.currentSelectedGameObject.gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().enabled)
        {
            AddColorButton(EventSystem.current.currentSelectedGameObject.gameObject);
        }
        else
        {
            RemoveColorButton(EventSystem.current.currentSelectedGameObject.gameObject);
        }
    }

    public void AddColorButton(GameObject colorButton)
    {
        _selectedColorButtons.Add(colorButton);
        _selectedColorButtonsColors.Add(colorButton.GetComponent<ColorButtonScript>().Color);
        _selectedColorButtonsNames.Add(colorButton.GetComponent<ColorButtonScript>().Name);
    }

    public void RemoveColorButton(GameObject colorButton)
    {
        _selectedColorButtons.Remove(colorButton);
        _selectedColorButtonsColors.Remove(colorButton.GetComponent<ColorButtonScript>().Color);
        _selectedColorButtonsNames.Remove(colorButton.GetComponent<ColorButtonScript>().Name);
    }

    //Deze methode is voor gemakkelijk, normaal en moeilijk. Deze methode vult '_selectedColorButtons' aan tot een specifiek aantal kleuren meegegeven met de parameter.
    //De methode geeft voorrang op kleuren die zijn gekozen in de instellingen.
    public void GenerateColorButtons(int amountOfColors)
    {
        while (amountOfColors < _selectedColorButtons.Count)
        {
            RemoveColorButton(_selectedColorButtons[Random.Range(0, _selectedColorButtons.Count)]);
        }
        while (amountOfColors > _selectedColorButtons.Count)
        {
            GameObject chosenColorButton = null;

            while (chosenColorButton == null || _selectedColorButtons.Contains(chosenColorButton))
            {
                chosenColorButton = _colorButtons[Random.Range(0, _colorButtons.Count)];
            }

            AddColorButton(chosenColorButton);
        }

        Debug.Log(_selectedColorButtons + "         " + _selectedColorButtons.Count.ToString());
    }

    //dit is nodig om de aantal waarden te weten dat true is in _colorButtonSettings
    public int GetAmountOfColors()
    {
        int amount = 0;

        foreach (GameObject colorButton in _colorButtons)
        {
            if (colorButton.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().enabled)
            {
                amount++;
            }
        }

        return amount;
    }
}
