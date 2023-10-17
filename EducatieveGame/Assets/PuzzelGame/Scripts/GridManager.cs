using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridManager2 : MonoBehaviour
{
    public int _width, _height;
    public Tile _tilePrefab;
    public Piece _piecePrefab;
    public Texture2D _sourceImage;
    public Transform _cam;
    private Dictionary<Vector2, Tile> _tiles;
    private Texture2D[,] _imagePieces;
    private Dictionary<Vector2, Piece> _pieces;

    // Start is called before the first frame update
    void Start()
    {
        CutImage();
        GenerateGrid();
        GeneratePieces();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = (_height * -1); y < 0; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector2(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {IntToChar(x)} {y * -1}";
                var isOffset = (x + (y * -1)) % 2 == 1;
                spawnedTile.Init(isOffset);
                spawnedTile.GetComponent<Tile>()._tmp.text = IntToChar(x).ToString() + (y * -1).ToString();
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, ((float)_height * -1) / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPos(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        else
        {
            return null;
        }
    }

    public void CutImage()
    {
        int pieceWidth = _sourceImage.width / _width;
        int pieceHeight = _sourceImage.height / _height;

        _imagePieces = new Texture2D[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _imagePieces[x, y] = new Texture2D(pieceWidth, pieceHeight);
                _imagePieces[x, y].Apply();
            }
        }
    }

    void GeneratePieces()
    {
        _pieces = new Dictionary<Vector2, Piece>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                int imagePiecesX = x;
                int imagePiecesY = y;

                Texture2D texturePiece = _imagePieces[imagePiecesX, imagePiecesY];

                Sprite spritePiece = Sprite.Create(texturePiece, new Rect(0, 0, texturePiece.width, texturePiece.height), Vector2.one * 0.5f);

                var spawnedPiece = Instantiate(_piecePrefab, new Vector2(x, y), Quaternion.identity);

                spawnedPiece.name = $"Piece {IntToChar(x)} {y + 1}";
                spawnedPiece.GetComponent<Piece>()._renderer.sprite = spritePiece;

                _pieces[new Vector2(x, y)] = spawnedPiece;
            }
        }
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, -(float)_height / 2 + 0.5f, -10);
    }

    private char IntToChar(int num)
    {
        switch (num)
        {
            case 0:
                return 'A';
            case 1:
                return 'B';
            case 2:
                return 'C';
            case 3:
                return 'D';
            case 4:
                return 'E';
            case 5:
                return 'F';
            case 6:
                return 'G';
            case 7:
                return 'H';
            case 8:
                return 'I';
            case 9:
                return 'J';
            case 10:
                return 'K';
            case 11:
                return 'L';
            case 12:
                return 'M';
            case 13:
                return 'N';
            case 14:
                return 'O';
            case 15:
                return 'P';
            case 16:
                return 'Q';
            case 17:
                return 'R';
            case 18:
                return 'S';
            case 19:
                return 'T';
            case 20:
                return 'U';
            case 21:
                return 'V';
            case 22:
                return 'W';
            case 23:
                return 'X';
            case 24:
                return 'Y';
            case 25:
                return 'Z';
        }
        return '!';
    }
}
