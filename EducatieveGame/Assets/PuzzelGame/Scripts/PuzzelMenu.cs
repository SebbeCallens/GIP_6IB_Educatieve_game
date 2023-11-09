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
    private GameObject[] _options;
    public PuzzelMenu _instance;
    public GameObject _imageArray;
    public Sprite _image;

    private void Start()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        StartGame();
        GenerateOptions();
        Back();
    }

    public void GenerateOptions()
    {
        _options = new GameObject[_imageArray.GetComponent<ImageArray>()._images.Length];
        var files = System.IO.Directory.GetFiles("Assets/PuzzelGame/Images/Puzzels", "?.png");
        for (int i = 0; i < _options.Length; i++)
        {
            var option = Instantiate(_optionPrefab, new Vector2(0, 0), Quaternion.identity);
            option.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0), false);
            _options[i] = option;
        }
        _options[0].GetComponent<Option>().setAll(1, 1, gameObject, _imageArray.GetComponent<ImageArray>()._images[0]);
        _options[1].GetComponent<Option>().setAll(2, 1, gameObject, _imageArray.GetComponent<ImageArray>()._images[1]);
        _options[2].GetComponent<Option>().setAll(3, 2, gameObject, _imageArray.GetComponent<ImageArray>()._images[2]);
        _options[3].GetComponent<Option>().setAll(4, 3, gameObject, _imageArray.GetComponent<ImageArray>()._images[3]);
        _options[4].GetComponent<Option>().setAll(5, 3, gameObject, _imageArray.GetComponent<ImageArray>()._images[4]);
        _options[5].GetComponent<Option>().setAll(8, 5, gameObject, _imageArray.GetComponent<ImageArray>()._images[5]);
        _options[6].GetComponent<Option>().setAll(16, 9, gameObject, _imageArray.GetComponent<ImageArray>()._images[6]);

        _options[0].transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(100, 100);
        _options[1].transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(150, 75);
        _options[2].transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(150, 100);
        _options[3].transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(120, 90);
        _options[4].transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(150, 90);
        _options[5].transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(120, 75);
        _options[6].transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(160, 90);
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
}
