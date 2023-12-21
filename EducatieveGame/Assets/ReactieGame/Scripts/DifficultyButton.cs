using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyButton : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _nextDescription;
    [SerializeField] private TextMeshProUGUI _nextText;
    [SerializeField] private GameObject _next;

    private string Name { get => _name; set => _name = value; }

    public void SetDifficulty(int difficulty) //stel moeilijkheid in
    {
        PlayerPrefs.SetInt(Name, difficulty);
        if (_next != null)
        {
            _nextText.text = _nextDescription;
            _next.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene("ReactionGame");
        }
    }
}
