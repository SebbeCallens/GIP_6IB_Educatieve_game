using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.IO;

public class OptionMaker : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _selectedImage;
    [SerializeField] private Sprite _img;
    [SerializeField] private GameObject _createBlock;
    private string _filePath;
    public GameObject Menu { get { return _menu; } }
    public GameObject SelectedImage { get { return _selectedImage; } }
    public Sprite Img { get { return _img; } set { _img = value; } }
    public GameObject CreateBlock { get { return _createBlock;} }

    public void ChooseImage()
    {
        //opens device directory
        _filePath = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
        if (_filePath != null)
        {
            //puts chosen image into the game
            WWW www = new WWW("file:///" + _filePath);
            Img = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.one * 0.5f);
            SelectedImage.GetComponent<UnityEngine.UI.Image>().sprite = Img;

            //scales the image preview
            double width = 600;
            double height = 600;
            if (www.texture.width > www.texture.height)
            {
                height = ((www.texture.height * 1.0) / (www.texture.width * 1.0)) * 600;
            }
            else
            {
                width = ((www.texture.width * 1.0) / (www.texture.height * 1.0)) * 600;
            }
            SelectedImage.transform.parent.localScale = new Vector3((float)width, (float)height, 1f);

            //unlocks the create button
            CreateBlock.SetActive(false);
        }
    }
   
    public void MakeOption()
    {
        Menu.GetComponent<PuzzelMenu>().AddImage(Img);
        Menu.GetComponent<PuzzelMenu>().CancelOptionCreation();
    }
}
