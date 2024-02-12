using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    [SerializeField] private GameObject _spawner;
    [SerializeField] private GameObject _destroyer;

    [SerializeField] private GameObject _conveyor;

    public static int _xValueStart = -10;
    public static int _xValueEnd = 10;
    public static int _yValue = -2;

    //dit script is voor het bijhouden van data te maken met de loopband
}
