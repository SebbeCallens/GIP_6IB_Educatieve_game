using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.IO;
using SFB;

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
        var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") };
        string[] filePaths = StandaloneFileBrowser.OpenFilePanel("Kies een afbeelding", "", extensions, false);
        if (filePaths != null && filePaths.Length > 0 && !string.IsNullOrEmpty(filePaths[0]))
        {
            string imagePath = filePaths[0];
            byte[] fileData = File.ReadAllBytes(imagePath);
            Texture2D texture = new(2, 2);
            texture.LoadImage(fileData);

            //puts chosen image into the game
            Img = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            SelectedImage.GetComponent<UnityEngine.UI.Image>().sprite = Img;

            //scales the image preview
            double width = 600;
            double height = 600;
            if (texture.width > texture.height)
            {
                height = ((texture.height * 1.0) / (texture.width * 1.0)) * 600;
            }
            else
            {
                width = ((texture.width * 1.0) / (texture.height * 1.0)) * 600;
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
