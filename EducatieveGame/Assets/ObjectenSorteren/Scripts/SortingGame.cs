using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SortingGame : MonoBehaviour
{
    [SerializeField] private Vector3[] _spawnLocations; //de locaties waar objecten spawnen
    [SerializeField] private Vector3 _conveyorSpawnLocation; //locatie waar object spawned op conveyor
    [SerializeField] private GameObject _sortingItem; //prefab sorteer object
    [SerializeField] private GameObject _sortingBox; //prefab sorteer box
    [SerializeField] private GameObject _trashcan; //de vuilbak
    [SerializeField] private TextMeshProUGUI _sortModeText; //de tekst van sorteermodus
    private Color[] _sortingColors; //de kleuren om mee te sorteren
    private Color[] _selectedSortingColors; //de geselecteerde kleuren om mee te sorteren
    private string[] _sortingTexts; //de kleur teksten om mee te sorteren
    private string[] _selectedSortingTexts; //de geselecteerde kleur teksten om mee te sorteren
    private bool _sortByColor = false; //of er volgen kleur wordt gesorteerd
    private bool _trashcanMode = false; //of de vuilbakmodus aan is
    private bool _conveyorMode = false; //of de loopbandmodus aan is
    private int _score = 0; //behaalde score

    private Vector3[] SpawnLocations { get => _spawnLocations; set => _spawnLocations = value; }
    private Vector3 ConveyorSpawnLocation { get => _conveyorSpawnLocation; set => _conveyorSpawnLocation = value; }
    private GameObject SortingItem { get => _sortingItem; set => _sortingItem = value; }
    private GameObject SortingBox { get => _sortingBox; set => _sortingBox = value; }
    private GameObject Trashcan { get => _trashcan; set => _trashcan = value; }
    private TextMeshProUGUI SortModeText { get => _sortModeText; set => _sortModeText = value; }
    private Color[] SortingColors { get => _sortingColors; set => _sortingColors = value; }
    private Color[] SelectedSortingColors { get => _selectedSortingColors; set => _selectedSortingColors = value; }
    private string[] SortingTexts { get => _sortingTexts; set => _sortingTexts = value; }
    private string[] SelectedSortingTexts { get => _selectedSortingTexts; set => _selectedSortingTexts = value; }
    public bool SortByColor { get => _sortByColor; private set => _sortByColor = value; }
    private bool TrashcanMode { get => _trashcanMode; set => _trashcanMode = value; }
    private bool ConveyorMode { get => _conveyorMode; set => _conveyorMode = value; }
    private int Score { get => _score; set => _score = value; }

    private void Awake() //spel starten met juiste instellingen
    {
        if (PlayerPrefs.GetInt("trashcan") == 1) //vuilbak instellen
        {
            TrashcanMode = true;
            Trashcan.SetActive(true);
            Trashcan.GetComponent<SortBox>().Create(Color.white, string.Empty, true);
        }

        if (PlayerPrefs.GetInt("conveyor") == 1) //loopband instellen
        {
            ConveyorMode = true;
        }

        int difficulty = MenuLogic.Difficulty + 1;

        //kleuren laden
        SortingColors = SortingMenuLogic.SortingColors;
        List<Color> selectedSortingColors = SortingMenuLogic.SelectedSortingColors.ToList();
        SortingTexts = SortingMenuLogic.SortingTexts;
        List<string> selectedSortingTexts = SortingMenuLogic.SelectedSortingTexts.ToList();
        SelectedSortingColors = new Color[difficulty];
        SelectedSortingTexts = new string[difficulty];

        for (int i = 0; i < difficulty; i++)
        {
            int randomIndex = Random.Range(0, selectedSortingColors.Count);

            SelectedSortingColors[i] = selectedSortingColors[randomIndex];
            SelectedSortingTexts[i] = selectedSortingTexts[randomIndex];

            selectedSortingColors.RemoveAt(randomIndex);
            selectedSortingTexts.RemoveAt(randomIndex);
        }

        //begin spawnen

        SpawnSortBoxes(MenuLogic.Difficulty + 1);

        if (!ConveyorMode)
        {
            SpawnSortItems();
        }

        SortModeText.text = "woord";
    }

    private void Update() //sorteer objecten op loopband spawnen
    {
        if (ConveyorMode)
        {

        }

        if (GameObject.FindWithTag("SortItem") == null)
        {
            SortByColor = !SortByColor;
            if (SortByColor)
            {
                SortModeText.text = "kleur";
            }
            else
            {
                SortModeText.text = "woord";
            }
            SpawnSortItems();
        }
    }

    public void ItemSorted() //item goed gesorteerd
    {
        Score++;
    }

    public void ItemLost() //item verloren gegaan of fout gesorteerd
    {
        Score--;
    }

    private void SpawnSortItems() //sorteer objecten spawnen
    {
        foreach (Vector3 spawnLocation in SpawnLocations)
        {
            if (TrashcanMode && Random.value > 0.75) //25% kans op vuilbak item als vuilbakmodus
            {
                int randomIndexColor = Random.Range(0, SortingColors.Length);
                int randomIndexText = Random.Range(0, SortingTexts.Length);

                while (SortByColor && SelectedSortingColors.Contains(SortingColors[randomIndexColor])) //ervoor zorgen dat het zeker vuilbak is
                {
                    randomIndexColor = Random.Range(0, SortingColors.Length);
                }
                while (!SortByColor && SelectedSortingTexts.Contains(SortingTexts[randomIndexText])) //ervoor zorgen dat het zeker vuilbak is
                {
                    randomIndexText = Random.Range(0, SortingTexts.Length);
                }

                SortItem sortItem = Instantiate(SortingItem, spawnLocation, Quaternion.identity, transform).GetComponent<SortItem>();
                sortItem.Create(SortingColors[randomIndexColor], SortingTexts[randomIndexText], true, false);
            }
            else //een sorteerobject spawnen dat niet in de vuilbak hoort
            {
                int randomIndexColor = Random.Range(0, SelectedSortingColors.Length);
                int randomIndexText = Random.Range(0, SelectedSortingTexts.Length);
                SortItem sortItem = Instantiate(SortingItem, spawnLocation, Quaternion.identity, transform).GetComponent<SortItem>();
                sortItem.Create(SelectedSortingColors[randomIndexColor], SelectedSortingTexts[randomIndexText], false, false);
            }
        }
    }

    private void SpawnSortBoxes(int amount) //de sorteer dozen spawnen
    {
        //hulp centreren dozen
        float startX = -((amount - 1) * 2.5f) / 2f;

        for (int i = 0; i < amount; i++)
        {
            SortBox sortBox = Instantiate(SortingBox, new Vector3(startX + i * 2.5f, 2f, 0f), Quaternion.identity, transform).GetComponent<SortBox>();
            sortBox.Create(SelectedSortingColors[i], SelectedSortingTexts[i], false);
        }
    }
}
