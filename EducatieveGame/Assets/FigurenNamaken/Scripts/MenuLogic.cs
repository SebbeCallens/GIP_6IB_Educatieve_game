using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] private GameObject[] _menus; //menu op index 0 is het standaard menu
    [SerializeField] private Toggle _assistToggle;
    [SerializeField] private Toggle _symmetricalToggle;
    [SerializeField] private Toggle _arrowsToggle;
    [SerializeField] private GameObject _confirmationWarning;
    [SerializeField] private bool _assist = false;
    [SerializeField] private GameObject _pokemonPuzzels;
    private bool _firstToggle = true; //dit zorgt ervoor dat wanneer de toggle ingesteld word dit niet de functie ToggleAssistMode triggered

    private GameObject[] Menus { get => _menus; set => _menus = value; }
    private Toggle AssistToggle { get => _assistToggle; set => _assistToggle = value; }
    private Toggle SymmetricalToggle { get => _symmetricalToggle; set => _symmetricalToggle = value; }
    private Toggle ArrowsToggle { get => _arrowsToggle; set => _arrowsToggle = value; }
    private GameObject ConfirmationWarning { get => _confirmationWarning; set => _confirmationWarning = value; }
    private bool Assist { get => _assist; set => _assist = value; }
    private GameObject PokemonPuzzels { get => _pokemonPuzzels; set => _pokemonPuzzels = value; }
    private bool FirstToggle { get => _firstToggle; set => _firstToggle = value; }

    private void Awake() //stel de toggle voor hulpmodus in
    {
        if (PokemonPuzzels != null && GameObject.FindWithTag("pokemonpuzzles") == null)
        {
            GameObject pokemonPuzzels = Instantiate(PokemonPuzzels);
            DontDestroyOnLoad(pokemonPuzzels);
        }

        if (Assist)
        {
            PlayerPrefs.SetInt("assist", 0);
            PlayerPrefs.SetInt("symmetrical", 0);
            PlayerPrefs.SetInt("arrows", 0);
        }

        if (ArrowsToggle != null)
        {
            PlayerPrefs.SetInt("puzzeldifficulty", 1);
        }

        if (AssistToggle != null)
        {
            if (PlayerPrefs.GetInt("assist") == 0)
            {
                AssistToggle.isOn = false;
            }
            else
            {
                AssistToggle.isOn = true;
            }
        }

        if (SymmetricalToggle != null)
        {
            if (PlayerPrefs.GetInt("symmetrical") == 0)
            {
                SymmetricalToggle.isOn = false;
            }
            else
            {
                SymmetricalToggle.isOn = true;
            }
        }

        if (ArrowsToggle != null)
        {
            if (PlayerPrefs.GetInt("arrows") == 0)
            {
                ArrowsToggle.isOn = false;
            }
            else
            {
                ArrowsToggle.isOn = true;
            }
        }
        FirstToggle = false;
    }

    public void LoadScene(string sceneName) //laad een scene met gegeven naam
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Close(int index) //sluit gegeven menu en toon het standaard menu
    {
        Menus[index].SetActive(false);
        Menus[0].SetActive(true);
    }

    public void Open(int index) //open gegeven menu en verberg het standaard menu
    {
        Menus[0].SetActive(false);
        Menus[index].SetActive(true);

        if (index == 1)
        {
            TextMeshProUGUI text = Menus[index].GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Wil je de figuur " + PlayerPrefs.GetString("figure") + " tekenen?";
            if (PlayerPrefs.GetInt("assist") == 1 && ConfirmationWarning != null)
            {
                ConfirmationWarning.SetActive(true);
            }
            else
            {
                ConfirmationWarning.SetActive(false);
            }
        }
    }

    public void ToggleAssistMode() //stel hulpmodus in
    {
        if (!FirstToggle)
        {
            if (PlayerPrefs.GetInt("assist") == 0)
            {
                PlayerPrefs.SetInt("assist", 1);
                if (ArrowsToggle != null && PlayerPrefs.GetInt("arrows") == 1)
                {
                    PlayerPrefs.SetInt("arrows", 0);
                    FirstToggle = true;
                    ArrowsToggle.isOn = false;
                    FirstToggle = false;
                }
            }
            else
            {
                PlayerPrefs.SetInt("assist", 0);
            }
        }
    }

    public void ToggleSymmetricalMode() //stel symmetrische modus in
    {
        if (!FirstToggle)
        {
            if (PlayerPrefs.GetInt("symmetrical") == 0)
            {
                PlayerPrefs.SetInt("symmetrical", 1);
                if (ArrowsToggle != null && PlayerPrefs.GetInt("arrows") == 1)
                {
                    PlayerPrefs.SetInt("arrows", 0);
                    FirstToggle = true;
                    ArrowsToggle.isOn = false;
                    FirstToggle = false;
                }
            }
            else
            {
                PlayerPrefs.SetInt("symmetrical", 0);
            }
        }
    }

    public void ToggleArrowsMode() //stel symmetrische modus in
    {
        if (!FirstToggle)
        {
            if (PlayerPrefs.GetInt("arrows") == 0)
            {
                PlayerPrefs.SetInt("arrows", 1);
                if (PlayerPrefs.GetInt("symmetrical") == 1)
                {
                    PlayerPrefs.SetInt("symmetrical", 0);
                    FirstToggle = true;
                    SymmetricalToggle.isOn = false;
                    FirstToggle = false;
                }
                if (PlayerPrefs.GetInt("assist") == 1)
                {
                    PlayerPrefs.SetInt("assist", 0);
                    FirstToggle = true;
                    AssistToggle.isOn = false;
                    FirstToggle = false;
                }
            }
            else
            {
                PlayerPrefs.SetInt("arrows", 0);
            }
        }
    }

    public void Quit() //sluit het spel af
    {
        Application.Quit();
    }
}
