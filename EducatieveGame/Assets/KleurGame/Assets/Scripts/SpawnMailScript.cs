using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMailScript : MonoBehaviour
{

    double _xScreenSize = 17;
    //double _yScreenSize = 10;

    [SerializeField] GameObject _organisingOnObject;

    [SerializeField] GameObject _mailbox;
    [SerializeField] GameObject _mailItem;
    [SerializeField] int _amountOfMailboxes = 3;
    [SerializeField] int _amountOfMailItems = 5;
    [SerializeField] int _amountOfMailItemsLeft;
    
    private int _points = 0;

    List<Color> _colors;
    List<string> _colorsString;
    
    // Start is called before the first frame update
    void Start()
    {
        //chooses the color of the mailbox
        //important!!! the possible color values have to be the same in MailScript in colors array!!!

        Color groen = new Color(0, 255, 0);
        Color rood = new Color(255, 0, 0);
        Color blauw = new Color(0, 0, 255);


        List<Color> allColors = new List<Color> { rood, groen, blauw };
        List<string> allColorsString = new List<string> { "rood", "groen", "blauw"};

        _colors = allColors;
        _colorsString = allColorsString;

        SpawnMailItems();
        SpawnMailboxes();
    }

    // Update is called once per frame
    void Update()
    {
        if (_amountOfMailItemsLeft == 0)
        {
            _organisingOnObject.GetComponent<SortingOnScript>().ChooseSortingMethod();
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

            newMailbox.GetComponent<SpriteRenderer>().color = _colors[0];
            newMailbox.GetComponent<MailChecker>().SetMailboxColor(_colorsString[0]);

            Debug.Log(_colors[0]);

            _colors.RemoveAt(0);
            _colorsString.RemoveAt(0);

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
