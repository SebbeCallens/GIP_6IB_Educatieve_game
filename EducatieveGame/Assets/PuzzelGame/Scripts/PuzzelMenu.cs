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
    public GameObject _menu;
    public GameObject _imgChooser;
    public GameObject _optionPrefab;
    public int _optionWidth;
    public int _optionHeight;
    private List<GameObject> _options = new();
    public PuzzelMenu _instance;
    public GameObject _imageArray;
    public Sprite _image;

    private void Start()
    {
        _instance = this;
    }

    public void GenerateOptions()
    {
        //_options = new GameObject[_imageArray.GetComponent<ImageArray>()._images.Length];
        var files = System.IO.Directory.GetFiles("Assets/PuzzelGame/Images/Puzzels", "?.png");
        for (int i = 0; i < _options.Count; i++)
        {
            var option = Instantiate(_optionPrefab, new Vector2(0, 0), Quaternion.identity);
            option.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0), false);
            _options[i] = option;
        }
    }

    public void StartGame()
    {
        _menu.SetActive(false);
        _imgChooser.SetActive(true);
    }

    public void Back()
    {
        _menu.SetActive(true);
        _imgChooser.SetActive(false);
    }

    public void Leave()
    {
        SceneManager.LoadScene(0);
    }
}
