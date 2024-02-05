using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenLogic : MonoBehaviour
{
    private static string _currentGame = "MainMenu";

    public static string CurrentGame { get => _currentGame; set => _currentGame = value; }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(CurrentGame);
    }
}
