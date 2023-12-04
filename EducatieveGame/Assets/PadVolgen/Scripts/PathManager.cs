using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private GameObject _tile;
    [SerializeField] private Transform _cam;
    private Dictionary<Vector2, PathTile> _tiles;
    private List<PathTile> _visitedTiles = new List<PathTile>();

    private void Awake()
    {
        GenerateGrid();
        GeneratePath(SpawnPlayer());
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

    public Vector2 GetTilePosition(PathTile tile)
    {
        foreach (var kvp in _tiles)
        {
            if (kvp.Value == tile)
            {
                return kvp.Key;
            }
        }

        return Vector2.zero;
    }

    public PathTile SpawnPlayer()
    {
        List<PathTile> _possibleTiles = new List<PathTile>();

        for (int x = 0; x < _width; x++)
        {
            _possibleTiles.Add(GetTileAtPosition(new(x, 0)));
            _possibleTiles.Add(GetTileAtPosition(new(x, _height -1)));
        }

        for (int y = 1; y < _height; y++)
        {
            _possibleTiles.Add(GetTileAtPosition(new(0, y)));
            _possibleTiles.Add(GetTileAtPosition(new(_width - 1, y)));
        }

        foreach (var tile in _possibleTiles)
        {
            tile.SetTile(Color.red, false, false, "");
        }

        PathTile _playerSpawn = _possibleTiles[Random.Range(0, _possibleTiles.Count)];
        _playerSpawn.SetTile(Color.blue, false, false, "");
        _playerSpawn.HasPlayer = true;
        return _playerSpawn;
    }

    public List<PathTile> GetPossibleTiles(PathTile currentTile)
    {
        Vector2 currentTilePos = GetTilePosition(currentTile);
        List<Vector2> adjacentPositions = new List<Vector2>
    {
        new Vector2(currentTilePos.x + 1, currentTilePos.y),
        new Vector2(currentTilePos.x - 1, currentTilePos.y),
        new Vector2(currentTilePos.x, currentTilePos.y + 1),
        new Vector2(currentTilePos.x, currentTilePos.y - 1)
    };

        List<PathTile> possibleTiles = adjacentPositions
            .Where(pos => GetTileAtPosition(pos) != null && !_visitedTiles.Contains(GetTileAtPosition(pos)))
            .Select(pos => GetTileAtPosition(pos))
            .ToList();

        return possibleTiles;
    }


    public void GeneratePath(PathTile spawnTile)
    {
        int length = Random.Range(400, 401);
        if (length > (_width * _height) / 2)
        {
            length = (_width * _height) / 2;
        }
        bool reset = false;
        PathTile currentTile = spawnTile;
        _visitedTiles.Add(currentTile);

        for (int i = 0; i < length; i++)
        {
            List<PathTile> _possibleTiles = GetPossibleTiles(currentTile);

            if (_possibleTiles.Count == 0)
            {
                reset = true;
            }

            else
            {
                foreach (var tile in _possibleTiles)
                {
                    tile.SetTile(Color.yellow, false, false, "");
                }

                currentTile = _possibleTiles[Random.Range(0, _possibleTiles.Count)];
                currentTile.SetTile(Color.grey, false, false, "");
                _visitedTiles.Add(currentTile);
            }
        }

        currentTile.SetTile(Color.cyan, false, false, "");
        currentTile.IsFinish = true;

        if (reset)
        {
            Reset();
        }
        else
        {
            GenerateObstacles();
        }
    }

    public void GenerateObstacles()
    {
        List<PathTile> _possibleTiles = new List<PathTile>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (!_visitedTiles.Contains(GetTileAtPosition(new(x, y))))
                {
                    _possibleTiles.Add(GetTileAtPosition(new(x, y)));
                }
            }
        }

        foreach (var tile in _possibleTiles)
        {
            if (Random.value > 0.5)
            {
                tile.SetTile(Color.black, true, false, "");
            }
        }
    }

    public void Reset()
    {
        Debug.Log("Generation fail");
        _visitedTiles.Clear();
        _tiles.Clear();
        for (int i = 0; i < transform.childCount;  i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        GenerateGrid();
        GeneratePath(SpawnPlayer());
        GenerateObstacles();
    }
}
