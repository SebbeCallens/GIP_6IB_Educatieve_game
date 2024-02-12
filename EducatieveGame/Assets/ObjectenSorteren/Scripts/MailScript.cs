using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MailScript : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private string _text;

    [SerializeField] private TextMeshPro _textObject;

    private Vector3 _originalPosition;


    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = transform.position;
    }

    void Update()
    {
        if (SettingsDataScript._conveyorSetting)
        {
            if (gameObject.GetComponent<DraggingScript>().Dragging)
            {
                PauseTween();
            }
            else if (gameObject.LeanIsPaused())
            {
                ContinueTween();
            }
            //zorgen dat objecten niet kunnen verschuiven worden (op y-waarde) bij een collision van een ander object
            else if (gameObject.transform.position.y != -2)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, -2, gameObject.transform.position.z);
            }
        }
    }

    private void Awake()
    {

    }

    //start het tweenen en wanneer de tween beeindigd is, verliest de speler punten en wordt het object vernietigd
    public void StartTween()
    {
        LeanTween.moveX(gameObject, ConveyorScript._xValueEnd, SettingsDataScript._conveyorSpeed).setOnComplete(DestroyAndLosePoints);
    }

    //pauzeert de tween als het nog niet op pauze stond
    private void PauseTween()
    {
        if (!gameObject.LeanIsPaused())
        {
            LeanTween.pause(gameObject);
        }
    }

    //het doorgaan van de tween als deze op pauze stond
    private void ContinueTween()
    {
        if (gameObject.LeanIsPaused())
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, -2, gameObject.transform.position.z);
            gameObject.LeanResume();
        }


        //timeleftafterdragged nog niet geimplementeerd! (_timeLeftAfterDragged = _timeLeftAfterDragged - tijd dat al is gebruikt voor de tween)
        //positie tussen -10 en 10          --> 0 en 20 voor gemakkelijk
        //positief maken door + 10
        //waarde = 4        --> 10 - 4 / 2

        /*int _timeLeftAfterDragged = SettingsDataScript._conveyorSpeed;

        _timeLeftAfterDragged = 10 - (_timeLeftAfterDragged + 10) / 2;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, ConveyorScript._yValue, 1);

        

        LeanTween.moveX(gameObject, ConveyorScript._xValueEnd, _timeLeftAfterDragged).setOnComplete(DestroyAndLosePoints);

        /*if (_timeLeftAfterDragged == 0)
        {
            MailChecker.LosePoints();
        }*/
    }

    //vernietigd het object en de speler verliest punten
    private void DestroyAndLosePoints()
    {
        Debug.Log("mailitem got to the end, losing points and destroying part");
        
        Destroy(gameObject);
        MailChecker.LosePoints();
    }

    public Color GetColor()
    {
        return _color;
    }

    public string GetText()
    {
        return _text;
    }

    public Vector3 GetoriginalPosition()
    {
        return _originalPosition;
    }

    public void SetColor(Color value)
    {
        _color = value;

        _textObject.color = value;
    }

    public void SetText(string value)
    {
        _text = value;

        _textObject.text = value;
    }
}
