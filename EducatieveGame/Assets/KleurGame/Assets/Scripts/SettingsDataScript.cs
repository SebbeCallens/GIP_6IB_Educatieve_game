using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsDataScript : MonoBehaviour
{
    public static bool _trashcanSetting = false;
    public static bool _testModeSetting = false;
    public static string _chosenDifficulty;


    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("SettingsMenu").SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleSettingsUI()
    {
        if (GameObject.Find("SettingsMenu"))
        {
            if (GameObject.Find("SettingsMenu").activeSelf)
            {
                GameObject.Find("SettingsMenu").SetActive(false);
            }
            else
            {
                GameObject.Find("SettingsMenu").SetActive(true);
            }
            
        }
    }

    public void ToggleTrashbinSetting(bool newValue)
    {
        _trashcanSetting = newValue;
    }

    public void ToggleTestModeSetting(bool newValue)
    {
        _testModeSetting = newValue;
    }

    public void EasyMode()
    {
        _chosenDifficulty = "easy";
        LoadKleurgameScene();
    }

    public void NormalMode()
    {
        _chosenDifficulty = "normal";
        LoadKleurgameScene();
    }

    public void DifficultMode()
    {
        _chosenDifficulty = "difficult";
        LoadKleurgameScene();
    }

    public void LoadKleurgameScene() { SceneManager.LoadScene("KleurGame"); }
}
