using UnityEngine;

public class DifficultyButton : MonoBehaviour
{
    public void SetDifficulty(int difficulty) //stel moeilijkheid in
    {
        PlayerPrefs.SetInt("difficulty", difficulty);
    }
}
