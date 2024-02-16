using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortingMenuLogic : MenuLogic
{
    [SerializeField] private GameObject[] _gameColors; //de kleuren voor het spel

    private static Color[] _sortingColors; //de kleuren voor het spel
    private static Color[] _selectedSortingColors; //de geselecteerde kleuren voor het spel
    private static string[] _sortingTexts; //de kleur teksten voor het spel
    private static string[] _selectedSortingTexts; //de geselecteerde kleur teksten voor het spel

    private GameObject[] GameColors { get => _gameColors; set => _gameColors = value; }
    public static Color[] SortingColors { get => _sortingColors; private set => _sortingColors = value; }
    public static Color[] SelectedSortingColors { get => _selectedSortingColors; private set => _selectedSortingColors = value; }
    public static string[] SortingTexts { get => _sortingTexts; private set => _sortingTexts = value; }
    public static string[] SelectedSortingTexts { get => _selectedSortingTexts; private set => _selectedSortingTexts = value; }

    private void Awake() //menu instellen
    {
        AwakeBase();
    }

    public void ToggleColor(int index) //kleur aan/uit zetten
    {
        GameColors[index].transform.GetChild(0).GetComponent<Image>().enabled = !GameColors[index].transform.GetChild(0).GetComponent<Image>().enabled;
    }

    public void LoadColors() //kleuren laden
    {
        List<Color> sortingColors = new();
        List<Color> selectedSortingColors = new();
        List<string> sortingTexts = new();
        List<string> selectedSortingTexts = new();
        for (int i = 0; i < GameColors.Length; i++)
        {
            if (GameColors[i].transform.GetChild(0).GetComponent<Image>().enabled)
            {
                sortingColors.Add(GameColors[i].transform.GetComponent<Image>().color);
                selectedSortingColors.Add(GameColors[i].transform.GetComponent<Image>().color);
                sortingTexts.Add(GameColors[i].transform.name);
                selectedSortingTexts.Add(GameColors[i].transform.name);
            }
            else
            {
                sortingColors.Add(GameColors[i].transform.GetComponent<Image>().color);
                sortingTexts.Add(GameColors[i].transform.name);
            }
        }

        SortingColors = sortingColors.ToArray();
        SelectedSortingColors = selectedSortingColors.ToArray();
        SortingTexts = sortingTexts.ToArray();
        SelectedSortingTexts = selectedSortingTexts.ToArray();
    }
}
