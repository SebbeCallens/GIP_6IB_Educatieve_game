using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortingOnScript : MonoBehaviour
{
    private string _sortingMethod = "";

    // Start is called before the first frame update
    void Start()
    {
        ChooseSortingMethod();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetSortingMethod()
    {
        return _sortingMethod;
    }

    //het kiezen van als er moet worden gesorteerd op woord of kleur & past het aan in de tekst
    private void ChooseSortingMethod()
    {
        string[] sortingMethods = new string[] { "woord", "kleur" };

        _sortingMethod = sortingMethods[Random.Range(0, sortingMethods.Length)];
        ChangeSortingText(_sortingMethod);
    }

    public void ChangeSortingText(string text)
    {
        gameObject.GetComponent<Text>().text = "Sorteren op: " + text;
    }
}
