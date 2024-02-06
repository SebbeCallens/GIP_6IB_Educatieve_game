using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsScript : MonoBehaviour
{
    //variabelen die moeten meegekregen worden van StartMenuScript, voor nu hard gecodeerd
    //private int _amountOfColors = 3;


    //private Color[] _allColors = new Color[] { Color.red, Color.green, Color.blue, Color.black, Color.yellow, new Color(1, 0.65f, 0), new Color(0.85f, 0.48f, 0.85f), new Color(1, 0.75f, 0.80f), Color.cyan, Color.red, Color.red, Color.red };
    //private string[] _allColorsString = new string[] { "rood", "groen", "blauw", "zwart", "geel", "oranje", "violet", "roze", "cyaan", "rood", "rood", "rood" };


    private string _sortingMethod = "";
    private Color[] _colors;
    private string[] _colorsString;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        ChooseSortingMethod();
        //ChooseColors();
    }

    // Update is called once per frame
    void Update()
    {

    }



    //het kiezen van als er moet worden gesorteerd op woord of kleur & past het aan in de tekst
    public void ChooseSortingMethod()
    {
        string[] sortingMethods = new string[] { "woord", "kleur" };

        _sortingMethod = sortingMethods[Random.Range(0, sortingMethods.Length)];

        GameObject.Find("OrganisingOnText").GetComponent<SortingOnScript>().ChangeSortingText(_sortingMethod);
    }

    //deze methode is voor zelf kleuren te kiezen (als de speler geen kleuren zelf kiest) (niet af)
    /*public void ChooseColors()
    {
        List<int> chosenIndexes = new List<int>();
        int chosenIndex;
        bool validIndex = false;

        for (int i = 0; i < _amountOfColors; i++)
        {
            validIndex = false;
            
            while (!validIndex)
            {
                chosenIndex = Random.Range(0, _allColors.Length - 1);

                if (!chosenIndexes.Contains(chosenIndex))
                {
                    validIndex = true;
                    chosenIndexes.Add(chosenIndex);

                    _colors[i] = _allColors[chosenIndex];
                    _colorsString[i] = _allColorsString[chosenIndex];

                    Debug.Log(_colors[i] + " " + _colorsString[i]);
                }
            }
        }
    }*/

    public Color[] GetColors()
    {
        return _colors;
    }

    public string[] GetColorsString()
    {
        return _colorsString;
    }

    public string GetSortingMethod()
    {
        return _sortingMethod;
    }
}
