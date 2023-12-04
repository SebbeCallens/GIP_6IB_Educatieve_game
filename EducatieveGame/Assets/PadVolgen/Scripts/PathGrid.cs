using System.Collections.Generic;
using UnityEngine;

//dit script houd het grid van het pad bij
public class PathGrid : MonoBehaviour
{
    [SerializeField] private int _width; //breedte grid
    [SerializeField] private int _height; //hoogte grid
    [SerializeField] private GameObject _tile; //een tile
    private Dictionary<Vector2, PathTile> _tiles; //de tiles van het grid met hun posities

    public int Width { get => _width; set => _width = value; }
    public int Height { get => _height; set => _height = value; }

    private void Awake() //dictionary posities met tiles aanmaken
    {
        _tiles = new Dictionary<Vector2, PathTile>();
    }

    public void GenerateGrid() //grid genereren
    {
        //oud grid verwijderen indien nodig
        _tiles.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        //nieuw grid maken
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                GameObject spawnedTile = Instantiate(_tile, new Vector2(x, y), Quaternion.identity, transform);
                spawnedTile.name = x + "-" + y;

                _tiles[new Vector2(x, y)] = spawnedTile.GetComponent<PathTile>();
                spawnedTile.GetComponent<PathTile>().SetTile(Color.green, false, false, "");
            }
        }

        //camera centreren op het grid
        Camera.main.transform.position = new Vector3((float)Width / 2 - 0.5f, (float)Height / 2 - 0.5f, -10);
    }

    public PathTile GetTileAtPosition(Vector2 pos) //tile op positie vinden
    {
        if (_tiles.TryGetValue(pos, out PathTile tile))
        {
            return tile;
        }

        return null;
    }

    public Vector2 GetTilePosition(PathTile tile) //positie van tile vinden
    {
        foreach (KeyValuePair<Vector2, PathTile> kvp in _tiles)
        {
            if (kvp.Value == tile)
            {
                return kvp.Key;
            }
        }

        return Vector2.zero;
    }
}
