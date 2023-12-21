using System.Collections;
using System.Collections.Generic;
using SFB;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class V02_Slicer : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject _puzzelPiece;
    public GameObject PuzzelPiece { get { return _puzzelPiece; } set { _puzzelPiece = value; } }

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
                GameObject imagePart = Instantiate(PuzzelPiece, Vector3.zero, Quaternion.identity, transform);

                //de schaal en sprite van het stuk instellen
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100);
                imagePart.GetComponent<Image>().sprite = sprite;
                imagePart.name = $"{col + 1}-{IntToChar(rows - row)}"; //naam van afbeelding stuk instellen met coordinaten
            }
        }

        return (columns, rows);
    }
    
    public char IntToChar(int num)
    {
        return (char)('A' + num - 1);
    }
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
