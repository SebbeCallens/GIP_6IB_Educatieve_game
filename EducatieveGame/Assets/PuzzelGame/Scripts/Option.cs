using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Option : MonoBehaviour
{
    [SerializeField] private Sprite _image;
    [SerializeField] private GameObject _confirm;

    void Start()
    {
        _confirm = GameObject.FindGameObjectWithTag("ToNextScene");
        CreateOption(gameObject.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite);
        gameObject.transform.GetChild(0).localScale = ScaleImage(360);
    }

    public Sprite Image { get { return _image; } set { _image = value; } }
    public GameObject Confirm { get { return _confirm; } }

    public Vector3 ScaleImage(double maxValue)
    {
        double width = maxValue;
        double height = maxValue;
        if (Image.texture.width > Image.texture.height)
        {
            height = ((Image.texture.height * 1.0) / (Image.texture.width * 1.0)) * maxValue;
        }
        else
        {
            width = ((Image.texture.width * 1.0) / (Image.texture.height * 1.0)) * maxValue;
        }
        return new Vector3((float)width, (float)height, 1f);
    }

    public void CreateOption(Sprite image)
    {
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = image;
        Image = image;
    }

    public void OptionClicked()
    {
        ConfirmScript.Image = Image;
        Confirm.transform.GetChild(0).gameObject.SetActive(true);
        Confirm.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = Image;
        Confirm.transform.GetChild(0).GetChild(0).GetChild(0).localScale = ScaleImage(500);
    }
}
