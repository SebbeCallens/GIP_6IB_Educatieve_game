using System;
using UnityEngine;

public class DifficultyButton : MonoBehaviour
{
    public void SetDifficulty(int difficulty) //moeilijkheidsgraad instellen
    {
        GameObject[] checks = GameObject.FindGameObjectsWithTag("Check");
        foreach (GameObject check in checks)
        {
            check.SetActive(false);
        }
        MenuLogic.SetDifficulty(difficulty);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SetCustomDifficulty(string difficulty) //moeilijkheidsgraad instellen
    {
        GameObject[] checks = GameObject.FindGameObjectsWithTag("Check");
        foreach (GameObject check in checks)
        {
            if (check.transform.parent.parent == transform.parent)
            {
                check.SetActive(false);
            }
        }
        string[] parts = difficulty.Split('-');
        PlayerPrefs.SetInt(parts[0], Convert.ToInt32(parts[1]));
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
