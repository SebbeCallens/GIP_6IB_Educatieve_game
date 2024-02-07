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
        MenuLogic.SetDifficulty(difficulty);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
