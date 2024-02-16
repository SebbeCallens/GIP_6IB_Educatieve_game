using UnityEngine;

public class CloseGameScript : MonoBehaviour
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
    }
}
