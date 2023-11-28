using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private GameObject _tile;
    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, PathTile> _tiles;

    private void Awake()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, PathTile>();
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tile, new Vector2(x, y), Quaternion.identity, transform);
                spawnedTile.name = x + "-" + y;

                _tiles[new Vector2(x, y)] = spawnedTile.GetComponent<PathTile>();
                spawnedTile.GetComponent<PathTile>().SetTile(Color.green, false, false , "");
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public PathTile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }
}
