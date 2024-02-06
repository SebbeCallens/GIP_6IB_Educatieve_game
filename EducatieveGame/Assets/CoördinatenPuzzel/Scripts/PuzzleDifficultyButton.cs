using UnityEngine;

public class PuzzleDifficultyButton : MonoBehaviour
{
    public void SetDifficulty(int difficulty)
    {
        GameObject[] checks = GameObject.FindGameObjectsWithTag("Check");
        foreach (GameObject check in checks)
        {
            check.SetActive(false);
        }
        PlayerPrefs.SetInt("puzzeldifficulty", difficulty);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
