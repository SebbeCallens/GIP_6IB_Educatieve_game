using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.WSA;

public class PuzzelMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _imgChooser;
    [SerializeField] private GameObject _optionPrefab;
    private static List<Sprite> _options = new();
    [SerializeField] private Sprite _image;
    [SerializeField] private GameObject _gridInViewport;
    [SerializeField] private GameObject _confirm;

    public GameObject Menu {  get { return _menu; } }
    public GameObject ImgChooser { get { return _imgChooser; } }
    public GameObject OptionPrefab {  get { return _optionPrefab; } }
    public static List<Sprite> Options { get { return _options; } }
    public Sprite Image { get { return _image; } set { _image = value; } }
    public GameObject GridInViewport { get { return _gridInViewport; } set { _gridInViewport = value; } }
    public GameObject Confirm { get { return _confirm; } }

    public void StartGame()
    {
        Menu.SetActive(false);
        ImgChooser.SetActive(true);
    }

    public void Back()
    {
        Menu.SetActive(true);
        ImgChooser.SetActive(false);
    }

    public void Leave()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenOptionCreation()
    {
        Confirm.transform.GetChild(1).gameObject.SetActive(true);
        //cleans the option maker
        OptionMaker maker = Confirm.transform.GetChild(1).GetComponent<OptionMaker>();
        maker.Img = null;
        maker.SelectedImage.GetComponent<UnityEngine.UI.Image>().sprite = null;
        maker.CreateBlock.SetActive(true);
    }
    public void CancelOptionCreation()
    {
        Confirm.transform.GetChild(1).gameObject.SetActive(false);
    }
    public void AddImage(Sprite image)
    {
        GameObject option = Instantiate(OptionPrefab, new Vector2(0f, 0f), Quaternion.identity, GridInViewport.transform);
        option.GetComponent<Option>().CreateOption(image);
    }
    public void CloseSetup()
    {
        Confirm.transform.GetChild(0).gameObject.SetActive(false);
    }
    public static void GenerateOptions()
    {
        if (Options.Count > 0)
        {
            PuzzelMenu menu = new();
            menu.StartGame();
            foreach (Sprite image in Options)
            {
                menu.AddImage(image);
            }
        }
    }
}
