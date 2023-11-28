using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManagerReference : MonoBehaviour
{
    [SerializeField] private ImageSlicer _slicer; //script dat de afbeelding snijdt
    [SerializeField] private GameObject _imageSlot; //slot voor een stukje
    [SerializeField] private Sprite _image; //de afbeelding
    [SerializeField] private float _scale = 1; //schaal grid
    [SerializeField] private int _width = 4; //aantal kolommen
    [SerializeField] private int _height = 4; //aantal rijen

    private ImageSlicer Slicer { get => _slicer; set => _slicer = value; }
    private GameObject ImageSlot { get => _imageSlot; set => _imageSlot = value; }
    private Sprite Image { get => _image; set => _image = value; }
    private int Width { get => _width; set => _width = value; }
    private int Height { get => _height; set => _height = value; }

    private void Awake()
    {
        //hier kan je nog scale, width, height en de afbeelding ophalen

        (int cols, int rows) = Slicer.SliceImage(Image.texture, Width, Height); //slice afbeelding met scale, aantal kolommen en aantal rijen

        List<GameObject> parts = new List<GameObject>(); //lijst van de de stukjes

        //lijsten instellen
        for (int i = 0; i < Slicer.transform.childCount; i++)
        {
            parts.Add(Slicer.transform.GetChild(i).gameObject);
        }

        //posities stukjes randomiseren
        for (int i = parts.Count - 1; i >= 0; i--)
        {
            int randomIndex = Random.Range(0, parts.Count);
            parts[randomIndex].transform.SetAsFirstSibling();
            parts.RemoveAt(randomIndex);
        }

        //grid met slots genereren
        //voor elke rij
        for (int row = 0; row < rows; row++)
        {
            //voor elke kolom
            for (int col = 0; col < cols; col++)
            {
                GameObject slot = Instantiate(ImageSlot, new(transform.position.x + (100 * col) - cols * 100 / 2f + 50, transform.position.y + (100 * row) - rows * 100 / 2f + 50), Quaternion.identity, transform);
                slot.name = col + "-" + (rows - 1 - row);
            }
        }
    }

    //doordat de naam van een slot en een afbeeldingstuk hun coordinaat zijn, kan je dit gebruiken om te controleren of het afbeeldingstuk in het juiste vakje staat
    //je kan scripts toevoegen aan het slot en aan de stukjes om de stukjes in de slots te kunnen slepen
    //de sprite van een slot stretched als de stukjes niet 1 op 1 zijn, hiervoor is er nog geen oplossing
    //offsets tussen rijen en kolommen instellen in de velden van de imageslicer
    //logica voor controle als de speler klaar is nog niet geimplementeerd
    //om de schaal juist de berekenen zodat de afbeelding altijd mooi past op het scherm is nog niet gedaan, deze word momenteel handmatig ingesteld
    //je zou een maximum breedte en hoogte kunnen instellen en dan via de dimensies van de afbeelding de schaal juist instellen
    //coordinaten in A B C D, 1 2 3 4, moet nog geimplementeerd worden
    //coordinaten waar het stukje komt moet nog boven elk stukje komen
}
