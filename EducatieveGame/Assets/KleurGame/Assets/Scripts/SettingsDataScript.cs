using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
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
    public static string _chosenDifficulty;

    public static List<bool> _colorButtonValues = new List<bool>();

    //referencing values
    [SerializeField] private GameObject _colorButton;
    [SerializeField] private UnityEngine.UI.Image _checkmark;

    //color setting variables
    private GameObject _colorsSetting;
    private GameObject _settingsMenu;
    private GameObject _normalMenu;
    private GameObject _colorMenu;
    private GameObject _colorButtonsObject;

    private GameObject _lastClickedColorButton;

    private List<GameObject> _colorButtons = new List<GameObject>();

    private int _xValueLastButton = -125;
    private int _yValue = 250;
    private int _increment = 125;

    // Start is called before the first frame update
    void Start()
    {
        DefineMenus();

        LoadColorSettings();
        UpdateColorButtonsList();
        
        //dit moet als laatste
        GameObject.Find("SettingsMenu").SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DefineMenus()
    {
        _colorsSetting = GameObject.Find("ColorsSetting");
        _settingsMenu = GameObject.Find("SettingsMenu");
        _normalMenu = GameObject.Find("NormalMenu");
        _colorMenu = GameObject.Find("ColorMenu");
        _colorButtonsObject = GameObject.Find("ColorButtons");
    }

    public void SettingsButton()
    {
        _normalMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        _colorMenu.SetActive(false);
    }

    public void UpdateColorButtonsList()
    {
        for (int i = 0; i < _colorButtonsObject.transform.childCount; i++)
        {
            _colorButtons.Add(_colorButtonsObject.transform.GetChild(i).gameObject);
        }
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

    
    //belangrijk: het 'ColorMenu' object moet overeenkomen met de juiste indexwaarde!!!
    private void ToggleColorMenu()
    {
        _colorMenu.SetActive(!_colorMenu.activeSelf);
    }

    //zorgt ervoor dat elke knop de juiste kleur krijgt.
    public void LoadColorSettings()
    {
        UnityEngine.UI.Image[] colorObjects = _colorMenu.transform.GetComponentsInChildren<UnityEngine.UI.Image>(true);

        foreach (UnityEngine.UI.Image colorObject in colorObjects)
        {
            if (colorObject.name.Contains("Red")) { colorObject.GetComponent<UnityEngine.UI.Image>().color = Color.red; }
            if (colorObject.name.Contains("Green")) { colorObject.GetComponent<UnityEngine.UI.Image>().color = Color.green; }
            if (colorObject.name.Contains("Blue")) { colorObject.GetComponent<UnityEngine.UI.Image>().color = Color.blue; }
            if (colorObject.name.Contains("Black")) { colorObject.GetComponent<UnityEngine.UI.Image>().color = Color.black; }
            if (colorObject.name.Contains("Yellow")) { colorObject.GetComponent<UnityEngine.UI.Image>().color = Color.yellow; }
        }
    }

    public void EasyMode()
    {
        _chosenDifficulty = "easy";
        LoadKleurgameScene();
    }

    public void NormalMode()
    {
        _chosenDifficulty = "normal";
        LoadKleurgameScene();
    }

    public void DifficultMode()
    {
        _chosenDifficulty = "difficult";
        LoadKleurgameScene();
    }

    public void ModifiedMode()
    {
        //code voor een zelfgekozen modus
    }

    public void LoadKleurgameScene() { SceneManager.LoadScene("KleurGame"); }

    //scrapped
    public void ColorMenuButtonClicked()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<UnityEngine.UI.Image>().color);

        //de knop togglet als de afbeelding active is of niet
        EventSystem.current.currentSelectedGameObject.gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().enabled = !EventSystem.current.currentSelectedGameObject.gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().enabled;

        //deze data verandering (hierboven) moet ergens opgeslaan worden!!! TBA


        //_lastClickedColorButton.GetComponent<UnityEngine.UI.Image>().color = EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<UnityEngine.UI.Image>().color;
    }

    public void ColorButtonClicked()
    {
        _lastClickedColorButton = EventSystem.current.currentSelectedGameObject;
        ToggleColorMenu();
    }

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
}
