using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] private GameObject[] _menus; //menu op index 0 is het standaard menu
    [SerializeField] private Toggle _assistToggle;
    [SerializeField] private GameObject _confirmationWarning;
    private bool _firstToggle = true; //dit zorgt ervoor dat wanneer de toggle ingesteld word dit niet de functie ToggleAssistMode triggered

    private GameObject[] Menus { get => _menus; set => _menus = value; }
    private Toggle AssistToggle { get => _assistToggle; set => _assistToggle = value; }
    public GameObject ConfirmationWarning { get => _confirmationWarning; set => _confirmationWarning = value; }
    private bool FirstToggle { get => _firstToggle; set => _firstToggle = value; }

    private void Awake() //stel de toggle voor hulpmodus in
    {
        if (AssistToggle != null && PlayerPrefs.HasKey("assist"))
        {
            if (PlayerPrefs.GetInt("assist") == 0)
            {
                AssistToggle.isOn = false;
            }
            else
            {
                AssistToggle.isOn = true;
            }
            FirstToggle = false;
        }
        else if (!PlayerPrefs.HasKey("assist"))
        {
            PlayerPrefs.SetInt("assist", 0);
        }
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
            if (PlayerPrefs.GetInt("assist") == 1)
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
            }
            else
            {
                PlayerPrefs.SetInt("assist", 0);
            }
        }
    }

    public void Quit() //sluit het spel af
    {
        Application.Quit();
    }
}
