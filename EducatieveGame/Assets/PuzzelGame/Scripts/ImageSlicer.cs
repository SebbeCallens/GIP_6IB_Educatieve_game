using SFB;
using System.IO;
using UnityEngine;

public class ImageSlicer : MonoBehaviour
{
    [SerializeField] private GameObject _imagePart; //prefab voor een deel van de afbeelding waar de texture van een gesneden stuk op komt te staan
    [SerializeField] private float _scale = 1; //dit bepaalt de grote van de afbeelding op het scherm
    [SerializeField] private int _width = 4; //in hoeveel kolommen dat de afbeelding in wordt gesneden
    [SerializeField] private int _height = 4; //in hoeveel rijen dat de afbeelding in wordt gesneden
    [SerializeField] private float _offsetX = 0; //lege afstand tussen elke kolom
    [SerializeField] private float _offsetY = 0; //lege afstand tussen elke rij

    private GameObject ImagePart { get => _imagePart; set => _imagePart = value; }
    private float Scale { get => _scale; set => _scale = value; }
    private int Width { get => _width; set => _width = value; }
    private int Height { get => _height; set => _height = value; }
    private float OffsetX { get => _offsetX; set => _offsetX = value; }
    private float OffsetY { get => _offsetY; set => _offsetY = value; }

    public void SelectImage() //functie om een afbeelding te selecteren om te snijden, deze kan je gebruiken in het menu als de speler zelf een afbeelding wilt kiezen
    {
        var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") }; //hier kan je meer afbeelding extensies toevoegen als dat nodig is

        string[] filePaths = StandaloneFileBrowser.OpenFilePanel("Kies een afbeelding", "C:\\Users\\Gebruiker\\Desktop\\School\\Informatica\\Programmeren\\GIP\\Educatieve Game\\GIP_6IB_Educatieve_game\\EducatieveGame\\Assets\\PuzzelGame\\Images\\Puzzels", extensions, false); //dit opent de standalone filebrowser

        if (filePaths != null && filePaths.Length > 0 && !string.IsNullOrEmpty(filePaths[0])) //nakijken of er effectief een afbeelding is gekozen en deze dan snijden
        {
            string imagePath = filePaths[0];

            byte[] fileData = File.ReadAllBytes(imagePath);
            Texture2D texture = new(2, 2);
            texture.LoadImage(fileData);
            SliceImage(texture);
        }
    }

    private void SliceImage(Texture2D image) //de afbeelding snijden
    {
        for (int i = 0; i < transform.childCount; i++) //haalt de oude stukken weg voor de nieuwe
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        //breedte en hoogte van 1 stuk
        int width = image.width / Width;
        int height = image.height / Height;

        float scale = 540f / image.height * Scale; //berekening schaal afbeelding
        int pixelsPerUnit = 100; //pixels per unit voor de afbeelding

        //offset voor breedte en hoogte berekenen en daarmee de afbeelding centreren
        float offsetX = -(Width * width * scale / pixelsPerUnit) / 2f + width * scale / pixelsPerUnit / 2f - OffsetX * ((Width - 1) / 2f) * Scale;
        float offsetY = -(Height * height * scale / pixelsPerUnit) / 2f + height * scale / pixelsPerUnit / 2f - OffsetY * ((Height - 1) / 2f) * Scale;
        transform.position = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);

        //voor elke rij
        for (int row = 0; row < Height; row++)
        {
            //voor elke kolom
            for (int col = 0; col < Width; col++)
            {
                //de texture voor het stuk instellen
                Texture2D texture = new(width, height);
                Color[] pixels = image.GetPixels(col * width, row * height, width, height);
                texture.SetPixels(pixels);
                texture.Apply();
                texture.filterMode = FilterMode.Point;

                //positie van het stuk berekenen
                float posX = transform.position.x + col * width * scale / pixelsPerUnit + col * OffsetX * Scale;
                float posY = transform.position.y + row * height * scale / pixelsPerUnit + row * OffsetY * Scale;
                Vector3 pos = new(posX, posY, transform.position.z);

                //het stuk instellen met gegeven positie
                GameObject imagePart = Instantiate(ImagePart, pos, Quaternion.identity, transform);

                //de schaal en sprite van het stuk instellen
                imagePart.transform.localScale = new(scale, scale, 1);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
                imagePart.GetComponent<SpriteRenderer>().sprite = sprite;
                imagePart.name = col + "-" + (Height - 1 - row); //naam van afbeelding stuk instellen met coordinaten
            }
        }
    }

    public void SliceImageAdvanced(Texture2D texture, float scale, int width, int height) //afbeelding snijden met schaal, aantal kolommen en aantal rijen
    {
        Scale = scale;
        Width = width;
        Height = height;
        SliceImage(texture);
    }
}
