using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    private int _width = 0;
    private int _height = 0;
    private GameObject _menu;
    private Sprite _image;

    private void setText()
    {
        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Width.ToString() + " x " + Height.ToString();
    }

    public void setAll(int width, int height, GameObject menu, Sprite image)
    {
        Width = width;
        Height = height;
        Menu = menu;
        Image = image;
        gameObject.transform.GetChild(0).GetComponent<Image>().sprite = Image;
    }

    public int Width {  get { return _width; } set { _width = value; setText(); } }
    public int Height { get { return _height; } set { _height = value; setText(); } }
    public GameObject Menu { get { return _menu; } set { _menu = value; } }
    public Sprite Image { get { return _image; } set { _image = value; } }

    public void PlayGame()
    {
        Menu.GetComponent<PuzzelMenu>()._optionWidth = Width;
        Menu.GetComponent<PuzzelMenu>()._optionHeight = Height;
        Menu.GetComponent<PuzzelMenu>()._image = Image;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
