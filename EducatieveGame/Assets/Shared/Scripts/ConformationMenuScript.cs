using UnityEngine;

public class ConformationMenuScript : MonoBehaviour
{
    private GameObject _conformationUI;

    void Start()
    {
        _conformationUI = this.gameObject;

        _conformationUI.SetActive(false);
    }

    //opent het conformation menu
    public void OpenConformationUI()
    {
        _conformationUI.SetActive(true);

        Time.timeScale = 0;
    }

    //code voor de 'Ja' optie in het conformation menu
    public void Quit()
    {
        Application.Quit();
    }

    //code voor de 'Nee' optie in het conformation menu
    public void Cancel()
    {
        _conformationUI.SetActive(false);

        Time.timeScale = 1;
    }
}
