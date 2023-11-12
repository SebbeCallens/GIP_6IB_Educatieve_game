using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortingOnScript : MonoBehaviour
{
    public void ChangeSortingText(string text)
    {
        gameObject.GetComponent<Text>().text = "Sorteren op: " + text;
    }
}
