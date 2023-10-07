using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshProUGUI _scoreText;
    private int _scoreCount;

    private TextMeshProUGUI ScoreText { get => _scoreText; set => _scoreText = value; }
    private int ScoreCount { get => _scoreCount; set => _scoreCount = value; }

    private void Awake() //score instellen
    {
        ScoreText = GetComponent<TextMeshProUGUI>();
        ScoreCount = 0;

        if (PlayerPrefs.GetInt("assist") == 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void AddScore(int score) //voeg score toe
    {
        ScoreCount += score;
        ScoreText.text = "Score: " + ScoreCount.ToString();
    }
}
