using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;
using Unity.VisualScripting;

public class V02_GridManager : MonoBehaviour
{
    [SerializeField] private GameObject _puzzelBox;
    private V02_Slicer _slicer;
    [SerializeField] private GameObject _puzzelHotbar;
    [SerializeField] private GameObject _puzzelSlot;
    [SerializeField] private GameObject _puzzelPiece;
    [SerializeField] private GameObject _sourceImage;
    [SerializeField] private Sprite _image;
    [SerializeField] private int _width = 4;
    [SerializeField] private int _height = 4;
    public GameObject PuzzelBox { get {  return _puzzelBox; } }
    public V02_Slicer Slicer { get { return _slicer; } set { _slicer = value; } }
    public GameObject PuzzelHotbar { get { return _puzzelHotbar; } }
    public GameObject PuzzelSlot { get {  return _puzzelSlot; } }
    public GameObject PuzzelPiece { get {  return _puzzelPiece; } }
    public GameObject SourceImage { get { return _sourceImage; } set { _sourceImage = value; } }
    public Sprite Image { get { return _image; } set { _image = value; } }
    public int Width { get { return _width; } set { _width = value; } }
    public int Height { get { return _height; } set { _height = value; } }

    void Start()
    {
        Slicer = PuzzelBox.GetComponent<V02_Slicer>();
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
                GameObject slot = Instantiate(PuzzelSlot, new(transform.position.x + (100 * col) - cols * 100 / 2f + 50, transform.position.y + (100 * row) - rows * 100 / 2f + 50), Quaternion.identity, transform);
                slot.name = $"{col}-{IntToChar(rows - 1 - row)}";
            }
        }
    }
    private char IntToChar(int num)
    {
        return (char)('A' + num - 1);
    }
}
