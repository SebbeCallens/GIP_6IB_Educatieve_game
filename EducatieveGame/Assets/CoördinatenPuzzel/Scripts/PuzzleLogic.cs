using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleLogic : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void Leave()
    {
        SceneManager.LoadScene("PuzzelGameMenu");
    }
}
