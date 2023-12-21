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
    private List<GameObject> _slots;
    [SerializeField] private GameObject _puzzelPiece;
    [SerializeField] private Sprite _sourceImage;
    [SerializeField] private int _width = 4;
    [SerializeField] private int _height = 4;
    public GameObject PuzzelBox { get {  return _puzzelBox; } }
    public V02_Slicer Slicer { get { return _slicer; } set { _slicer = value; } }
    public GameObject PuzzelHotbar { get { return _puzzelHotbar; } }
    public GameObject PuzzelSlot { get {  return _puzzelSlot; } }
    public GameObject PuzzelPiece { get {  return _puzzelPiece; } }
    public Sprite SourceImage { get { return _sourceImage; } set { _sourceImage = value; } }
    public int Width { get { return _width; } set { _width = value; } }
    public int Height { get { return _height; } set { _height = value; } }
    public List<GameObject> Slots { get { return _slots; } set { _slots = value; } }

    void Start()
    {
        Slicer = PuzzelBox.GetComponent<V02_Slicer>();
        Slots = new();

        //hier kan je nog scale, width, height en de afbeelding ophalen
        (int cols, int rows) = Slicer.SliceImage(SourceImage.texture, Width, Height); //slice afbeelding met scale, aantal kolommen en aantal rijen

        //lijst van de de stukjes
        List<GameObject> parts = new();

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
            parts[randomIndex].transform.SetParent(PuzzelHotbar.transform);
            parts.RemoveAt(randomIndex);
        }

        //grid met slots genereren
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject slot = Instantiate(PuzzelSlot, new Vector2(0f, 0f), Quaternion.identity, PuzzelBox.transform);
                slot.name = $"{col + 1}-{Slicer.IntToChar(row + 1)}";
                Slots.Add(slot);
            }
        }
    }
    public int ReturnScore()
    {
        int achieved = 0;
        foreach (GameObject slot in Slots)
        {
            if (slot.transform.childCount > 0 && slot.transform.GetChild(0).name.Equals(slot.name))
            {
                achieved++;
            }
        }
        return achieved;
    }
}