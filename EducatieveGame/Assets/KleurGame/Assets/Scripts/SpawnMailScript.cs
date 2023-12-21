using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnMailScript : MonoBehaviour
{

    double _xScreenSize = 17;
    //double _yScreenSize = 10;

    //[SerializeField] GameObject _organisingOnObject;
    [SerializeField] GameObject _gameScriptManager;

    [SerializeField] GameObject _mailbox;
    [SerializeField] GameObject _mailItem;
    [SerializeField] int _amountOfMailItems = 5;
    [SerializeField] int _amountOfMailItemsLeft;

    private Color[] _colors;
    private string[] _colorsString;

    // Start is called before the first frame update
    void Start()
    {   
        _colors = _gameScriptManager.GetComponent<StatsScript>().GetColors();
        _colorsString = _gameScriptManager.GetComponent<StatsScript>().GetColorsString();

        /*Debug.Log("lengte _colors: " + _colors.Length);
        Debug.Log("lengte _colorsString: " + _colors.Length);

        for (int i = 0; i < _colors.Length; i++)
        {
            Debug.Log("Kleur: " + _colors[i]);
            Debug.Log("KleurString: " + _colorsString[i]);
        }*/

        SpawnMailItems();
        SpawnMailboxes();
    }

    // Update is called once per frame
    void Update()
    {
        if (_amountOfMailItemsLeft == 0)
        {
            _gameScriptManager.GetComponent<StatsScript>().ChooseSortingMethod();
            GenerateNewMail();
        }
    }
    public void SpawnMailItems()
    {
        _amountOfMailItemsLeft = _amountOfMailItems;
        
        //yValue: de y-waarde waarop alle mailItem elementen spawnen    distanceBetween: de hoeveelheid plek tussen elk mailItem    currentXValue: de x-waarde van het volgende mailItem element
        double yValue = -4;
        double distanceBetween = _xScreenSize / _amountOfMailItems;
        double currentXValue = distanceBetween / 2;

        for (int i = 0; i < _amountOfMailItems; i++)
        {
            GameObject newMailItem = Instantiate(_mailItem);
            newMailItem.transform.position = new Vector3((float) (currentXValue - _xScreenSize / 2), (float) yValue, 1);

            int colorIndex = Random.Range(0, SettingsDataScript._selectedColorButtons.Count);
            int textIndex = Random.Range(0, SettingsDataScript._selectedColorButtons.Count);

            newMailItem.GetComponent<MailScript>().SetColor(new Color(SettingsDataScript._selectedColorButtonsColors[colorIndex].r, SettingsDataScript._selectedColorButtonsColors[colorIndex].g, SettingsDataScript._selectedColorButtonsColors[colorIndex].b));
            newMailItem.GetComponent<MailScript>().SetText(new string (SettingsDataScript._selectedColorButtonsNames[textIndex]));

            currentXValue += distanceBetween;
        }
    }

    public void SpawnMailboxes()
    {
        double yValue = 0;
        double distanceBetween = _xScreenSize / SettingsDataScript._selectedColorButtons.Count;
        double currentXValue = distanceBetween / 2;

        for (int i = 0; i < SettingsDataScript._selectedColorButtons.Count; i++)
        {

            GameObject newMailbox = Instantiate(_mailbox);
            newMailbox.transform.position = new Vector3((float) (currentXValue - _xScreenSize / 2), (float) yValue, 1);

            newMailbox.GetComponent<SpriteRenderer>().color = new Color(SettingsDataScript._selectedColorButtonsColors[i].r, SettingsDataScript._selectedColorButtonsColors[i].g, SettingsDataScript._selectedColorButtonsColors[i].b);
            newMailbox.GetComponent<MailChecker>().SetMailboxColor(new string(SettingsDataScript._selectedColorButtonsNames[i]));

            currentXValue += distanceBetween;
        }
    }

    public void GenerateNewMail()
    {
        _amountOfMailItemsLeft = _amountOfMailItems;
        SpawnMailItems();
    }

    public int GetAmountOfMailItemsLeft()
    {
        return _amountOfMailItemsLeft;
    }

    public void SetAmountOfMailItemsLeft(int value)
    {
        _amountOfMailItemsLeft = value;
    }
}
