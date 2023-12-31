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
    [SerializeField] private GameObject _corner;
    private List<GameObject> _slots;
    [SerializeField] private GameObject _puzzelPiece;
    [SerializeField] private GameObject _coordinaatSlot;
    [SerializeField] private Sprite _sourceImage;
    [SerializeField] private int _width = 4;
    [SerializeField] private int _height = 4;
    public GameObject PuzzelBox { get {  return _puzzelBox; } }
    public V02_Slicer Slicer { get { return _slicer; } set { _slicer = value; } }
    public GameObject PuzzelHotbar { get { return _puzzelHotbar; } }
    public GameObject PuzzelSlot { get {  return _puzzelSlot; } }
    public GameObject PuzzelPiece { get {  return _puzzelPiece; } }
    public GameObject CoordinaatSlot { get { return _coordinaatSlot; } }
    public Sprite SourceImage { get { return _sourceImage; } set { _sourceImage = value; } }
    public int Width { get { return _width; } set { _width = value; } }
    public int Height { get { return _height; } set { _height = value; } }
    public List<GameObject> Slots { get { return _slots; } set { _slots = value; } }
    public GameObject Corner { get { return _corner; } }

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
        for (int row = -1; row < rows; row++)
        {
            for (int col = -1; col < cols; col++)
            {
                if (row == -1 && col == -1)
                {
                    GameObject topLeft = Instantiate(Corner, new Vector2(0f, 0f), Quaternion.identity, PuzzelBox.transform);
                    topLeft.name = "Corner";
                }
                else if (row == -1)
                {
                    GameObject coordinaat = Instantiate(CoordinaatSlot, new Vector2(0f, 0f), Quaternion.identity, PuzzelBox.transform);
                    coordinaat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{col + 1}";
                    coordinaat.name = $"{col + 1}";
                }
                else if (col == -1)
                {
                    GameObject coordinaat = Instantiate(CoordinaatSlot, new Vector2(0f, 0f), Quaternion.identity, PuzzelBox.transform);
                    coordinaat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{Slicer.IntToChar(row + 1)}";
                    coordinaat.name = $"{Slicer.IntToChar(row + 1)}";
                }
                else
                {
                    GameObject slot = Instantiate(PuzzelSlot, new Vector2(0f, 0f), Quaternion.identity, PuzzelBox.transform);
                    slot.name = $"{col + 1}-{Slicer.IntToChar(row + 1)}";
                    Slots.Add(slot);
                }
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
            else if (slot.transform.childCount > 0)
            {
                Color red = new();
                red.g = 0;
                red.b = 0;
                red.r = 225;
                red.a = 225;
                slot.transform.GetChild(0).GetComponent<Image>().color = red;
            }
        }
        return achieved;
    }
    public void ClearRedPaint()
    {
        foreach (GameObject slot in Slots)
        {
            if (slot.transform.childCount > 0 && !(slot.transform.GetChild(0).name.Equals(slot.name)))
            {
                Color white = new();
                white.g = 225;
                white.b = 225;
                white.r = 225;
                white.a = 225;
                slot.transform.GetChild(0).GetComponent<Image>().color = white;
            }
        }
    }
}
