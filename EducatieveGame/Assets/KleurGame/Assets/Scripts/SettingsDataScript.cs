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
    public static bool _testModeSetting = false;

    public static bool _timerSetting = true;
    public static int _timerValue = 60;

    public static string _chosenDifficulty;

    public static int _pointsPerAnswer = 1;

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
    private TextMeshPro _errorTextObject;

    //color setting variables
    private GameObject _colorsSetting;
    private GameObject _settingsMenu;
    private GameObject _normalMenu;
    private GameObject _colorMenu;
    private GameObject _colorButtonsObject;

    private GameObject _timerInputFieldObject;

    private GameObject _lastClickedColorButton;

    private int _xValueLastButton = -125;
    private int _yValue = 250;
    private int _increment = 125;

    // Start is called before the first frame update
    void Start()
    {
        ResetData();
        DefineMenus();
        SetColorButtonsList();

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

    public void DefineMenus()
    {
        _colorsSetting = GameObject.Find("ColorsSetting");
        _settingsMenu = GameObject.Find("SettingsMenu");
        _normalMenu = GameObject.Find("NormalMenu");
        _colorMenu = GameObject.Find("ColorMenu");
        _colorButtonsObject = GameObject.Find("ColorButtons");

        _timerInputFieldObject = GameObject.Find("TijdInputField");

        _errorTextObject = GameObject.Find("ErrorTextObject").GetComponent<TextMeshPro>();
    }

    public void SettingsButton()
    {
        if (_normalMenu.activeSelf)
        {
            _normalMenu.SetActive(false);
            _settingsMenu.SetActive(true);
            _colorMenu.SetActive(false);
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
            Debug.Log(currentColorButton);
        }
    }

    public static void ResetData()
    {
        _trashcanSetting = false;
        _testModeSetting = false;

        _timerSetting = true;
        _timerValue = 60;

        _pointsPerAnswer = 1;

        _chosenDifficulty = null;

        _selectedColorButtons = new List<GameObject>();
        _selectedColorButtonsColors = new List<Color>();
        _selectedColorButtonsNames = new List<string>();

        _colorButtons = new List<GameObject>();
        _colorButtonsColors = new List<Color>();
        _colorButtonsNames = new List<string>();

        MailChecker.Points = 0;
}

    public void LoadKleurgameScene()
    {
        for (int i = 0; i < _selectedColorButtons.Count; i++)
        {
            Debug.Log("object: " + _selectedColorButtons[i] + "\n" + 
            "color: " + _selectedColorButtonsColors[i] + "\n" + 
            "name: " + _selectedColorButtonsNames[i]);
        }

        SceneManager.LoadScene("KleurGame");
    }

    public void ToggleSettingsUI()
    {
        if (GameObject.Find("SettingsMenu"))
        {
            if (GameObject.Find("SettingsMenu").activeSelf)
            {
                GameObject.Find("SettingsMenu").SetActive(false);
            }
            else
            {
                GameObject.Find("SettingsMenu").SetActive(true);
            }
            
        }
    }

    public void ToggleTrashbinSetting(bool newValue)
    {
        _trashcanSetting = newValue;
    }

    public void ToggleTestModeSetting(bool newValue)
    {
        _testModeSetting = newValue;
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

    //kennelijk illegaal om aan te passen
    public void ChangeTime()
    {
        Debug.Log(_inputField.text);

        if (int.TryParse(_inputField.text, out int time))
        {
            Debug.Log(time);

            if (time > 0 && time < 1000)
            {
                _timerValue = time;
            }
        }
    }

    
    //belangrijk: het 'ColorMenu' object moet overeenkomen met de juiste indexwaarde!!!
    private void ToggleColorMenu()
    {
        _colorMenu.SetActive(!_colorMenu.activeSelf);
    }

    /*IEnumerator DisplayErrorText(string errorText)
    {
        Debug.Log(_errorTextObject);

        _errorTextObject.text = errorText;

        yield return new WaitForSeconds(3);

        _errorTextObject.text = "";
    }*/

    //tekst tonen en weer weghalen lukt nog niet.
    public bool CheckStatsValid()
    {
        bool statsValid = true;

        Debug.Log(_selectedColorButtons.Count);
        
        //checken als de gebruiker kleuren heeft gekozen
        if (_selectedColorButtons.Count == 1)
        {
            //DisplayErrorText("Er moeten minimaal 2 kleuren gekozen worden.");

            statsValid = false;
        }
        else if (_selectedColorButtons.Count <= 0)
        {
            //DisplayErrorText("Kies kleuren in instellingen.");

            statsValid = false;
        }
        else if (_selectedColorButtons.Count > 5)
        {
            //DisplayErrorText("Er mogen maximaal maar 5 kleuren gekozen worden.");

            statsValid = false;
        }

        return statsValid;
    }

    public void EasyMode()
    {
        _chosenDifficulty = "easy";
        _pointsPerAnswer = 1;

        _timerSetting = true;
        _timerValue = 60;
        _trashcanSetting = false;

        GenerateColorButtons(3);

        LoadKleurgameScene();
    }

    public void NormalMode()
    {
        _chosenDifficulty = "normal";
        _pointsPerAnswer = 2;

        _timerSetting = true;
        _timerValue = 60;
        _trashcanSetting = false;

        GenerateColorButtons(4);

        LoadKleurgameScene();
    }

    public void DifficultMode()
    {
        _chosenDifficulty = "difficult";
        _pointsPerAnswer = 3;

        _timerSetting = true;
        _timerValue = 60;
        _trashcanSetting = true;

        GenerateColorButtons(5);

        LoadKleurgameScene();
    }

    public void ModifiedMode()
    {
        //code voor een zelfgekozen modus
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
    ///De methode geeft voorrang op kleuren die zijn gekozen in de instellingen.
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

    public void ColorButtonClicked()
    {
        _lastClickedColorButton = EventSystem.current.currentSelectedGameObject;
        ToggleColorMenu();
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

    /*
    //verwijdert het laatste toegevoegde knopje
    public void RemoveColorButtonClicked()
    {
        //list opschonen
        _colorButtons.RemoveAll(item => item == null);

        //nieuwst toegevoegde knop verwijderen
        Destroy(_colorButtons[_colorButtons.Count - 1]);
        _colorButtons.RemoveAt(_colorButtons.Count - 1);

        _xValueLastButton -= _increment;

        //code voor + & - knop aan te passen
        ChangePositionPlusMinButtons(-_increment);
    }
    
    public void AddColorButtonClicked()
    {
        //code voor knop zelf
        GameObject newColorButton = Instantiate(_colorButton);
        newColorButton.transform.SetParent(_colorButtonsObject.transform);
        newColorButton.transform.position = new Vector3(_xValueLastButton + _increment, _yValue, 0);
        
        _colorButtons.Add(newColorButton);

        _xValueLastButton += _increment;

        //code voor + & - knop aan te passen
        ChangePositionPlusMinButtons(_increment);
    }
    
     //code voor + & - knop aan te passen
    private void ChangePositionPlusMinButtons(int xValue)
    {
        GameObject addColorButton = _colorsSetting.transform.Find("AddColorButton").gameObject;
        GameObject removeColorButton = _colorsSetting.transform.Find("RemoveColorButton").gameObject;

        addColorButton.transform.position = new Vector3(addColorButton.transform.position.x + xValue, addColorButton.transform.position.y, addColorButton.transform.position.z);
        removeColorButton.transform.position = new Vector3(removeColorButton.transform.position.x + xValue, removeColorButton.transform.position.y, removeColorButton.transform.position.z);
    }
    */
}
