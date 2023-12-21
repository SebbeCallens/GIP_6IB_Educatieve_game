using SFB;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageSlicer : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject _puzzlePart; //prefab voor een deel van de afbeelding waar de texture van een gesneden stuk op komt te staan

    private GameObject PuzzlePart { get => _puzzlePart; set => _puzzlePart = value; }

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
            SliceImage(texture, 1, 1);
        }
    }

    public (int,int) SliceImage(Texture2D image, int maxColumns, int maxRows) //de afbeelding snijden
    {
        float targetAspect = (float)maxColumns / maxRows;
        float imageAspect = (float)image.width / image.height;

        int columns, rows;

        if (imageAspect > targetAspect)
        {
            // Landscape image
            columns = Mathf.Min(maxColumns, image.width);
            rows = Mathf.RoundToInt(columns / imageAspect);
        }
        else
        {
            // Portrait image
            rows = Mathf.Min(maxRows, image.height);
            columns = Mathf.RoundToInt(rows * imageAspect);
        }

        //breedte en hoogte van 1 stuk
        int width = image.width / columns;
        int height = image.height / rows;

        //voor elke rij
        for (int row = 0; row < rows; row++)
        {
            //voor elke kolom
            for (int col = 0; col < columns; col++)
            {
                //de texture voor het stuk instellen
                Texture2D texture = new(width, height);
                Color[] pixels = image.GetPixels(col * width, row * height, width, height);
                texture.SetPixels(pixels);
                texture.Apply();
                texture.filterMode = FilterMode.Point;

                //het stuk instellen met gegeven positie
                GameObject imagePart = Instantiate(PuzzlePart, Vector3.zero, Quaternion.identity, transform);

                //de schaal en sprite van het stuk instellen
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100);
                imagePart.GetComponent<Image>().sprite = sprite;
                imagePart.name = col + "-" + (rows - 1 - row); //naam van afbeelding stuk instellen met coordinaten
            }
        }

        return (columns, rows);
    }

    //zorgen dat je stukjes terug kunt steken
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped.GetComponent<PuzzlePiece>() != null)
        {
            PuzzlePiece piece = dropped.GetComponent<PuzzlePiece>();
            piece.ParentAfterDrag = transform;
        }
    }
}
