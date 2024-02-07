using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class MenuLogic : MonoBehaviour
{
    [SerializeField] private GameObject[] _menus; //de menus
    [SerializeField] private Toggle[] _settingsToggles; //toggles van de instellingen
    [SerializeField] private string[] _settings; //namen instellingen
    [SerializeField] private bool _resetDifficulty; //of dit menu de moeilijkheidsgraad reset
    private bool _fromScript = false; //of toggle uit script getoggled wordt
    private int _currentMenu = 0; //index van huidig menu

    private static int _difficulty = 1; //de moeilijkheidsgraad

    protected GameObject[] Menus { get => _menus; set => _menus = value; }
    protected Toggle[] SettingsToggles { get => _settingsToggles; set => _settingsToggles = value; }
    protected string[] Settings { get => _settings; set => _settings = value; }
    protected bool ResetDifficulty { get => _resetDifficulty; set => _resetDifficulty = value; }
    protected int CurrentMenu { get => _currentMenu; set => _currentMenu = value; }
    protected bool FromScript { get => _fromScript; set => _fromScript = value; }
    public static int Difficulty { get => _difficulty; private set => _difficulty = value; }

    private void Awake() //difficulty op standaardwaarde en settings toggles instellen
    {
        if (ResetDifficulty)
        {
            Difficulty = 1;
        }
        FromScript = true;
        for (int i = 0; i < SettingsToggles.Length; i++)
        {
            if (PlayerPrefs.GetInt(Settings[i]) == 1)
            {
                SettingsToggles[i].isOn = true;
            }
        }
        FromScript = false;
    }

    public virtual void OpenMenu(int index) //open menu met index
    {
        Menus[CurrentMenu].SetActive(false);
        Menus[index].SetActive(true);
        CurrentMenu = index;
    }

    public void Close(int index) //sluit gegeven menu en toon het standaard menu
    {
        Menus[index].SetActive(false);
        Menus[0].SetActive(true);
    }

    public void ToggleSetting(string setting) //instelling bijwerken
    {
        if (!FromScript)
        {
            if (PlayerPrefs.GetInt(setting) == 1)
            {
                PlayerPrefs.SetInt(setting, 0);
            }
            else
            {
                PlayerPrefs.SetInt(setting, 1);
            }
        }
    }
    public void LoadScene(string sceneName) //laad een scene met gegeven naam
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit() //sluit het spel af
    {
        Application.Quit();
    }

    public static void SetDifficulty(int difficulty) //difficulty instellen
    {
        Difficulty = difficulty;
    }
}
