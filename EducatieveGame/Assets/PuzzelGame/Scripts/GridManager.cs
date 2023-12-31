using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.SearchService;
using Unity.VisualScripting;
using SFB;

public class GridManager2 : MonoBehaviour
{
    public int _width, _height;
    public GameObject _slotPrefab;
    public GameObject _puzzelBoxPrefab;
    public GameObject _piecePrefab;
    public GameObject _pieceBoxPrefab;
    public Sprite _sourceImage;
    private GameObject[,] _pieces;
    private GameObject _puzzelBox;
    public GameObject _logic;
    private PuzzelMenu _menu;

    //C�dric als je dit leest: sorry voor de spaghetti code...

    void Start()
    {
        _menu = GameObject.FindGameObjectWithTag("MenuScript").GetComponent<PuzzelMenu>();
        _sourceImage = _menu.GetComponent<PuzzelMenu>()._image;
        _width = _menu._optionWidth;
        _height = _menu._optionHeight;
        if (_width == _height)
        {
            _width = 5;
            _height = 5;
        }
        else if (_width == 2 && _height == 1)
        {
            _width = 10;
            _height = 5;
        }
        else if (_width == 3 && _height == 2)
        {
            _width = 9;
            _height = 6;
        }
        else if (_width == 4 && _height == 3)
        {
            _width = 8;
            _height = 6;
        }
        else if (_width == 5 && _height == 3)
        {
            _width = 10;
            _height = 6;
        }
        _pieces = new GameObject[_width, _height];
        GenerateBoxes();
        _logic.GetComponent<LogicScript>().MoveMenus();
    }

    public void GenerateBoxes()
    {
        //Default Box size = 950 x 950
        //Default Grid Cell size = 103 x 103
        //Default Slot Cell size = 58 x 58
        float w, h;
        float csx, csy;
        int x = 475, y = 0;
        // 1x1
        if (_width == _height)
        {
            w = 1f;
            h = 1f;
            csx = 165f / 103f;
            csy = csx;
        }
        // 2x1
        else if (_width == 10 && _height == 5)
        {
            w = 1050f / 950;
            h = 525f / 950f;
            csx = 83f / 103f;
            csy = 170f / 103f;
            x = 0;
            y = 270;
        }
        // 3x2
        else if (_width == 9 && _height == 6)
        {
            w = 945f / 950f;
            h = 630f / 950f;
            csx = 91f / 103f;
            csy = 142f / 103f;
        }
        // 4x3
        else if (_width == 8 && _height == 6)
        {
            w = 950f / 950f;
            h = 713f / 950f;
            csx = 1f;
            csy = 140f / 103f;
        }
        // 5x3
        else if (_width == 10 && _height == 6)
        {
            w = 950f / 950f;
            h = 570f / 950f;
            csx = 83f / 103f;
            csy = 141f / 103f;
        }
        // 8x5
        else if (_width == 8 && _height == 5)
        {
            w = 950f / 950f;
            h = 630f / 950f;
            csx = 1f;
            csy = 158f / 103f;
        }
        else if (_width == 16 && _height == 9)
        {
            w = 1.2f;
            h = 563f / 950f;
            csx = 51f / 103f;
            csy = 91f / 103f;
            x = 0;
            y = -275;
        }
        // uh oh
        else
        {
            w = 0.5f;
            h = 0.5f;
            csx = 0.5f;
            csy = 0.5f;
            x = 0;
            y = 0;
        }
        var PuzzelBox = Instantiate(_puzzelBoxPrefab, new Vector2(-x, -y), Quaternion.identity);
        var PiecesBox = Instantiate(_pieceBoxPrefab, new Vector2(x, y), Quaternion.identity);
        PuzzelBox.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        PiecesBox.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        PuzzelBox.transform.SetSiblingIndex(2);
        PiecesBox.transform.SetSiblingIndex(3);
        PuzzelBox.name = $"Puzzel Doos";
        PiecesBox.name = $"Stukken Doos";
        PuzzelBox.transform.localScale = new Vector2(w, h);
        PiecesBox.transform.localScale = new Vector2(w, h);
        PuzzelBox.GetComponent<PuzzelBoxScript>().BuildSlotMatrix(_width, _height);
        PiecesBox.GetComponent<PieceBoxScript>().BuildSlotMatrix(_width, _height);
        PuzzelBox.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(103f * csx, 103f * csy);
        PiecesBox.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(103f * csx, 103f * csy);

        Sprite image = _sourceImage;
        float imgWidth = image.rect.width / _width;
        float imgHeight = image.rect.height / _height;

        for (int i = 1; i <= _width; i++)
        {
            for (int j = 1; j <= _height; j++)
            {
                var PuzzelSlot = Instantiate(_slotPrefab, new Vector2(0, 0), Quaternion.identity);
                var PieceSlot = Instantiate(_slotPrefab, new Vector2(0, 0), Quaternion.identity);
                PuzzelSlot.transform.SetParent(PuzzelBox.transform.GetChild(0), false);
                PieceSlot.transform.SetParent(PiecesBox.transform.GetChild(0), false);
                PuzzelSlot.name = $"Puzzel Slot {IntToChar(i)} {j}";
                PieceSlot.name = $"Stuk Slot {IntToChar(i)} {j}";
                PuzzelSlot.GetComponent<Slot>().Coords = IntToChar(i).ToString() + j.ToString();
                PieceSlot.GetComponent<Slot>().Coords = "n/a";
                //PuzzelSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PuzzelSlot.GetComponent<Slot>().Coords;
                //PieceSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                PuzzelSlot.GetComponent<GridLayoutGroup>().cellSize = new Vector2(58f * csx, 58f * csy);
                PieceSlot.GetComponent<GridLayoutGroup>().cellSize = new Vector2(58f * csx, 58f * csy);
                //PuzzelSlot.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(58f * csx, 58f * csy);
                //PieceSlot.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(58f * csx, 58f * csy);
                PuzzelBox.GetComponent<PuzzelBoxScript>().Slots[i - 1, j - 1] = PuzzelSlot;
                PiecesBox.GetComponent<PieceBoxScript>().Slots[i - 1, j - 1] = PieceSlot;
                var PuzzelPiece = Instantiate(_piecePrefab, new Vector2(0, 0), Quaternion.identity);
                PuzzelPiece.name = $"Puzzel Stuk {IntToChar(i)} {j}";
                PuzzelPiece.GetComponent<Piece>().Coords = IntToChar(i).ToString() + j.ToString();

                Texture2D sprite = new Texture2D((int)imgWidth, (int)imgHeight);
                Color[] pixels = SpriteToTexture2DConverter(image).GetPixels(i * (int)imgWidth, j * (int)imgHeight, (int)imgWidth, (int)imgHeight);
                sprite.SetPixels(pixels);
                sprite.Apply();
                sprite.filterMode = FilterMode.Point;
                PuzzelPiece.GetComponent<Piece>().Img = sprite.ConvertTo<Sprite>();
                
                _pieces[i - 1, j - 1] = PuzzelPiece;
                if (i == _width && j == _height)
                {
                    for (int col = 0; col < _width; col++)
                    {
                        for (int row = 0; row < _height; row++)
                        {
                            int pbCol = Random.Range(0, _width);
                            int pbRow = Random.Range(0, _height);
                            if (PiecesBox.GetComponent<PieceBoxScript>().Slots[pbCol, pbRow].transform.childCount == 0)
                            {
                                _pieces[col, row].transform.SetParent(PiecesBox.GetComponent<PieceBoxScript>().Slots[pbCol, pbRow].transform, false);
                            }
                            else
                            {
                                row--;
                            }
                        }
                    }
                }
            }
        }
        _puzzelBox = PuzzelBox;
    }

    public string ReturnScore()
    {
        int maxScore = _width * _height;
        int currentScore = 0;
        foreach (GameObject Slot in _puzzelBox.GetComponent<PuzzelBoxScript>().Slots)
        {
            if (Slot.transform.childCount == 1)
            {
                if (Slot.transform.GetChild(0).GetComponent<Piece>().Coords.Equals(Slot.GetComponent<Slot>().Coords))
                {
                    currentScore++;
                }
            }
        }
        int procent = (currentScore / maxScore) * 100;
        string score = "Je hebt " + currentScore.ToString() + "/" + maxScore.ToString() + " gescoort. (" + procent.ToString() + "%)";
        return score;
    }

    Texture2D SpriteToTexture2DConverter(Sprite sprite)
    {
        // Create a new Texture2D with the same dimensions as the sprite
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

        // Get the pixels from the sprite and apply them to the texture
        texture.SetPixels(sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height));

        // Apply changes to the texture
        texture.Apply();

        return texture;
    }

    private char IntToChar(int num)
    {
        return (char)('A' + num -1);
        /**switch (num)
        {
            case 1:
                return 'A';
            case 2:
                return 'B';
            case 3:
                return 'C';
            case 4:
                return 'D';
            case 5:
                return 'E';
            case 6:
                return 'F';
            case 7:
                return 'G';
            case 8:
                return 'H';
            case 9:
                return 'I';
            case 10:
                return 'J';
            case 11:
                return 'K';
            case 12:
                return 'L';
            case 13:
                return 'M';
            case 14:
                return 'N';
            case 15:
                return 'O';
            case 16:
                return 'P';
            case 17:
                return 'Q';
            case 18:
                return 'R';
            case 19:
                return 'S';
            case 20:
                return 'T';
            case 21:
                return 'U';
            case 22:
                return 'V';
            case 23:
                return 'W';
            case 24:
                return 'X';
            case 25:
                return 'Y';
            case 26:
                return 'Z';
        }
        return '!';**/
    }
}
