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
    [SerializeField] int _amountOfMailboxes = 3;
    [SerializeField] int _amountOfMailItems = 5;
    [SerializeField] int _amountOfMailItemsLeft;
    
    private int _points = 0;

    private Color[] _colors;
    private string[] _colorsString;

    // Start is called before the first frame update
    void Start()
    {   
        _colors = _gameScriptManager.GetComponent<StatsScript>().GetColors();
        _colorsString = _gameScriptManager.GetComponent<StatsScript>().GetColorsString();

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

            int colorIndex = Random.Range(0, _colors.Length);
            int textIndex = Random.Range(0, _colorsString.Length);

            _mailItem.GetComponent<MailScript>().SetColor(_colors[colorIndex]);
            _mailItem.GetComponent<MailScript>().SetText(_colorsString[textIndex]);

            Debug.Log(textIndex);

            currentXValue += distanceBetween;
        }
    }

    public void SpawnMailboxes()
    {
        double yValue = 0;
        double distanceBetween = _xScreenSize / _amountOfMailboxes;
        double currentXValue = distanceBetween / 2;

        for (int i = 0; i < _amountOfMailboxes; i++)
        {
            GameObject newMailbox = Instantiate(_mailbox);
            newMailbox.transform.position = new Vector3((float) (currentXValue - _xScreenSize / 2), (float) yValue, 1);

            newMailbox.GetComponent<SpriteRenderer>().color = _colors[i];
            newMailbox.GetComponent<MailChecker>().SetMailboxColor(_colorsString[i]);

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

    public int GetPoints()
    {
        return _points;
    }

    public void SetPoints(int value)
    {
        if (GetPoints() + value >= 0)
        {
            _points = value;
        }
        else
        {
            _points = 0;
        }
    }
}
