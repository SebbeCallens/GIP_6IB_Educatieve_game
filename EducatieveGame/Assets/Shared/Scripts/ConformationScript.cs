using UnityEngine;

public class ConformationScript : MonoBehaviour
{
    [SerializeField] private GameObject _conformationUI;

    void Start()
    {
        _conformationUI.SetActive(false);
    }

    //opent het conformation menu
    public void OpenConformationUI()
    {
        _conformationUI.SetActive(true);
        ToggleTime();
    }

    //sluit het spel
    public void CloseGame()
    {
        Application.Quit();
    }

    //code voor de 'verdergaanknop' in het conformation menu
    public void Cancel()
    {
        _conformationUI.SetActive(false);
        ToggleTime();
    }

    public void ToggleTime()
    {
        if (_conformationUI.activeSelf)
        {
            Time.timeScale = 0;
            Debug.Log("Tijd is uit.");
        }
        else
        {
            Time.timeScale = 1;
            Debug.Log("Tijd is aan.");
        }
    }
}
