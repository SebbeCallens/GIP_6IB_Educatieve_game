using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MailChecker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] BoxCollider2D boxCollider;

    [SerializeField] GameObject _organisingOnTextObject;
    [SerializeField] GameObject _pointsCounterObject;
    [SerializeField] GameObject _gameScriptManager;

    [SerializeField] string _mailboxColor;

    private static int _points = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        _organisingOnTextObject = GameObject.FindGameObjectWithTag("TextObject");
        _gameScriptManager = GameObject.FindGameObjectWithTag("GameScriptManager");
        _pointsCounterObject = GameObject.Find("PointsCounter");
    }

    //het kijken als er een mailobject wordt geraakt met de mailbox of trashbin
    //checkt daarna welke sorting method er werd gekozen
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MailItem"))
        {
            //checkt als het object een mailbox of een trashbin is en voert dan desbetreffende code uit
            if (this.gameObject.CompareTag("Mailbox"))
            {
                MailboxCode(collision);
            }
            else if (this.gameObject.CompareTag("Trashbin"))
            {
                Debug.Log("Trashbin touched by mailItem.");
                TrashbinCode(collision);
            }
        }
    }

    //uitgevoerde code voor mailbox in de OncollisionEnter2D methode
    private void MailboxCode(Collision2D collision)
    {
        //checkt als er wordt gesorteerd op kleur of tekst
        if (_gameScriptManager.GetComponent<StatsScript>().GetSortingMethod().Equals("kleur"))
        {
            //checkt als de kleur van de postbus overeenkomt met de kleur van het woord
            if (collision.gameObject.GetComponent<MailScript>().GetColor() == gameObject.GetComponent<SpriteRenderer>().color)
            {
                Destroy(collision.gameObject);
                GainPoints();
            }
            else
            {
                collision.gameObject.GetComponent<Dragging>().SetDragging(false);
                collision.gameObject.transform.position = collision.gameObject.GetComponent<MailScript>().GetoriginalPosition();

                LosePoints();
            }
        }
        else
        {
            //checkt verder als de kleur van de postbus overeenkomt met het woord
            if (collision.gameObject.GetComponent<MailScript>().GetText() == _mailboxColor)
            {
                Destroy(collision.gameObject);
                GainPoints();
            }
            else
            {
                collision.gameObject.GetComponent<Dragging>().SetDragging(false);
                collision.gameObject.transform.position = collision.gameObject.GetComponent<MailScript>().GetoriginalPosition();

                LosePoints();
            }
        }
    }

    //uitgevoerde code voor trashbin in de OncollisionEnter2D methode
    private void TrashbinCode(Collision2D collision)
    {
        //checkt als de kleur van het mailitem gevonden is in alle geselecteerde kleuren
        foreach (Color selectedColor in SettingsDataScript._selectedColorButtonsColors)
        {
            Debug.Log(SettingsDataScript._selectedColorButtonsColors.Count);
            Debug.Log(collision.gameObject.GetComponent<MailScript>().GetColor());
            Debug.Log(selectedColor);
            
            //een nieuwe kleur wordt aangemaakt omdat de 4e parameter soms verschillend is.
            if (new Color(selectedColor.r, selectedColor.g, selectedColor.b, 0) == new Color(collision.gameObject.GetComponent<MailScript>().GetColor().r, collision.gameObject.GetComponent<MailScript>().GetColor().g, collision.gameObject.GetComponent<MailScript>().GetColor().b, 0))
            {
                Debug.Log("color is in selectedcolorbuttons");

                LosePoints();
                return;
            }
        }

        Debug.Log("color is not in selectedcolorbuttons");

        Destroy(collision.gameObject);
        GainPoints();
    }

    //verhoogt de score met 1 en past de pointscounter aan
    public void GainPoints()
    {
        SpawnMailScript spawnMailScript = _gameScriptManager.GetComponent<SpawnMailScript>();

        //aantal mailitem objecten - 1 doen.
        spawnMailScript.SetAmountOfMailItemsLeft(spawnMailScript.GetAmountOfMailItemsLeft() - 1);

        //aantal punten verhogen met 1.
        Points += SettingsDataScript._pointsPerAnswer;

        _pointsCounterObject.GetComponent<Text>().text = "punten: " + Points.ToString();
    }

    //verlaagt de score met 1 en past de pointscounter aan
    public void LosePoints()
    {
        SpawnMailScript spawnMailScript = _gameScriptManager.GetComponent<SpawnMailScript>();

        //aantal punten verlagen met 1.
        Points-= SettingsDataScript._pointsPerAnswer;

        _pointsCounterObject.GetComponent<Text>().text = "punten: " + Points.ToString();
    }

    //getters & setters

    public string MailboxColor
    {
        get { return _mailboxColor; }
        set { _mailboxColor = value; }
    }

    public static int Points
    {
        get { return _points; }
        set 
        { 
            if (value > 0)
            {
                _points = value;
            }
            else
            {
                _points = 0;
            }
        }
    }
}
