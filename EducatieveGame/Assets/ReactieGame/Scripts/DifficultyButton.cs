using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyButton : MonoBehaviour
{
    public void LoadDifficulty(int difficulty) //stel moeilijkheid in en laad het spel
    {
        PlayerPrefs.SetInt("difficulty", difficulty);
        SceneManager.LoadScene("ReactionGame");
    }
}
