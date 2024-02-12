using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
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
    [SerializeField] GameObject _trashbin;
    [SerializeField] int _amountOfMailItems = 5;
    [SerializeField] int _amountOfMailItemsLeft;

    [SerializeField] Sprite[] _mailImages;

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


        if (SettingsDataScript._conveyorSetting)
        {
            if (SettingsDataScript._trashcanSetting)
            {
                SpawnTrashbin();
            }

            SpawnMailboxes();
            StartCoroutine(SpawnMailItemsConveyorMode());
        }
        else if (SettingsDataScript._trashcanSetting)
        {
            SpawnMailItems();
            SpawnMailboxes();
            SpawnTrashbin();
        }
        
        else
        {
            SpawnMailItems();
            SpawnMailboxes();
        }
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
        double yValue = -2;
        double distanceBetween = _xScreenSize / _amountOfMailItems;
        double currentXValue = distanceBetween / 2;

        if (!SettingsDataScript._conveyorSetting)
        {
            //de uitgevoerde code voor een spel zonder loopbandmodus aan

            for (int i = 0; i < _amountOfMailItems; i++)
            {
                //als de vuilnisbak niet is aangevinkt, dan kiest het alleen maar uit de gekozen kleuren.
                //als de vuilnisbak is aangevinkt in settings, dan wordt er (mogelijks) andere mail gevormd.

                if (!SettingsDataScript._trashcanSetting)
                {
                    //uitvoerbare code voor een spel zonder modus aan

                    GenerateMailItemFromChosenColors().transform.position = new Vector3((float)(currentXValue - _xScreenSize / 2), (float)yValue, 1);
                }
                else
                {
                    //uitvoerbare code voor een spel met vuilnisbakmodus aan
                    //kans dat een mailItem een andere kleur heeft is +- 25% kans
                    if (Random.Range(0, 4) >= 1)
                    {
                        GenerateMailItemFromChosenColors().transform.position = new Vector3((float)(currentXValue - _xScreenSize / 2), (float)yValue, 1);
                    }
                    else
                    {
                        GenerateRandomMailItem().transform.position = new Vector3((float)(currentXValue - _xScreenSize / 2), (float)yValue, 1);
                    }
                }

                currentXValue += distanceBetween;
            }
        }
        else
        {

        }
        
    }

    IEnumerator SpawnMailItemsConveyorMode()
    {
        Debug.Log("SpawnMailItemsConveyorMode started!");
        
        while (true)
        {
            GameObject newMailItem;

            if (SettingsDataScript._trashcanSetting)
            { 
                newMailItem = GenerateRandomMailItem();
            }
            else
            { 
                newMailItem = GenerateMailItemFromChosenColors(); 
            }

            newMailItem.transform.position = new Vector3(ConveyorScript._xValueStart, ConveyorScript._yValue, 1);
            newMailItem.GetComponent<MailScript>().StartTween();
            Debug.Log("Spawned a mailItem at " + newMailItem.transform.position);

            yield return new WaitForSeconds(SettingsDataScript._timeBetweenObjects);
        }
    }

    //genereert een nieuw mailitem met een gekozen kleur van de gebruiker
    private GameObject GenerateMailItemFromChosenColors()
    {
        int colorIndex;
        int textIndex;

        GameObject newMailItem = Instantiate(_mailItem);
        newMailItem.GetComponent<SpriteRenderer>().sprite = GenerateRandomMailImage();
        newMailItem.GetComponent<SpriteRenderer>().color = Color.gray;

        colorIndex = Random.Range(0, SettingsDataScript._selectedColorButtons.Count);
        textIndex = Random.Range(0, SettingsDataScript._selectedColorButtons.Count);

        newMailItem.GetComponent<MailScript>().SetColor(new Color(SettingsDataScript._selectedColorButtonsColors[colorIndex].r, SettingsDataScript._selectedColorButtonsColors[colorIndex].g, SettingsDataScript._selectedColorButtonsColors[colorIndex].b));
        newMailItem.GetComponent<MailScript>().SetText(new string(SettingsDataScript._selectedColorButtonsNames[textIndex]));

        return newMailItem;
    }

    private GameObject GenerateRandomMailItem() 
    {
        int colorIndex;
        int textIndex;

        GameObject newMailItem = Instantiate(_mailItem);
        newMailItem.GetComponent<SpriteRenderer>().sprite = GenerateRandomMailImage();
        newMailItem.GetComponent<SpriteRenderer>().color = Color.gray;

        colorIndex = Random.Range(0, SettingsDataScript._colorButtonsColors.Count);
        textIndex = Random.Range(0, SettingsDataScript._colorButtonsNames.Count);

        newMailItem.GetComponent<MailScript>().SetColor(new Color(SettingsDataScript._colorButtonsColors[colorIndex].r, SettingsDataScript._colorButtonsColors[colorIndex].g, SettingsDataScript._colorButtonsColors[colorIndex].b));
        newMailItem.GetComponent<MailScript>().SetText(new string(SettingsDataScript._colorButtonsNames[textIndex]));

        return newMailItem;
    }

    private Sprite GenerateRandomMailImage()
    {
        return _mailImages[Random.Range(0, _mailImages.Length)];
    }

    public void SpawnMailboxes()
    {
        double yValue = 2f;
        double distanceBetween = _xScreenSize / SettingsDataScript._selectedColorButtons.Count;
        double currentXValue = distanceBetween / 2;

        for (int i = 0; i < SettingsDataScript._selectedColorButtons.Count; i++)
        {

            GameObject newMailbox = Instantiate(_mailbox);
            newMailbox.transform.position = new Vector3((float) (currentXValue - _xScreenSize / 2), (float) yValue, 1);

            newMailbox.GetComponent<SpriteRenderer>().color = new Color(SettingsDataScript._selectedColorButtonsColors[i].r, SettingsDataScript._selectedColorButtonsColors[i].g, SettingsDataScript._selectedColorButtonsColors[i].b);
            newMailbox.GetComponent<MailChecker>().MailboxColor = new string(SettingsDataScript._selectedColorButtonsNames[i]);

            currentXValue += distanceBetween;
        }
    }

    public void SpawnTrashbin()
    {
        double yValue = -4f;
        double xValue = -7f;

        GameObject newMailbox = Instantiate(_trashbin);
        newMailbox.transform.position = new Vector3((float) xValue, (float) yValue, 1);
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
