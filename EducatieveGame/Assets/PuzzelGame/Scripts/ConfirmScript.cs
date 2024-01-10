using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmScript : MonoBehaviour
{
    private static Sprite _image;
    private static int _width;
    private static int _height;
    private static Boolean _strechedPuzzle;
    [SerializeField] private GameObject _widthInput;
    [SerializeField] private GameObject _heightInput;
    [SerializeField] private GameObject _playBlock;

    void Start()
    {
        Width = 4;
        Height = 4;
        StrechedPuzzle = false;
    }

    public static Sprite Image {  get { return _image; } set { _image = value; } }
    public static int Width { get { return _width; } set { _width = value; } }
    public static int Height { get { return _height; } set { _height = value; } }
    public static Boolean StrechedPuzzle { get { return _strechedPuzzle; }  set { _strechedPuzzle = value; } }
    public GameObject WidthInput { get { return _widthInput; } }
    public GameObject HeightInput { get { return _heightInput; } }
    public GameObject PlayBlock { get { return _playBlock; } }

    public void StartPuzzle()
    {
        if (Width > 1 && Height > 1 && Image != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            ErrMsg.SetActive(true);
        }
    }

    public void CheckValues()
    {
        string width = WidthInput.GetComponent<TMP_InputField>().text;
        string height = HeightInput.GetComponent<TMP_InputField>().text;
        if (width != null && height != null)
        {
            int w = Convert.ToInt32(width);
            int h = Convert.ToInt32(height);
            if (w > 1 && h > 1)
            {
                Width = w;
                Height = h;
                PlayBlock.SetActive(false);
            }
            else
            {
                Width = 0;
                Height = 0;
                PlayBlock.SetActive(true);
            }
        }
    }
}
