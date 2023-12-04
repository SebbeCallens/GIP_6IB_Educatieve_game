using System.Collections.Generic;
using UnityEngine;

//dit script zorgt voor een pad in het grid
public class PathManager : MonoBehaviour
{
    [SerializeField] private int _minLength; //minimum lengte pad
    [SerializeField] private int _maxLength; //maximum lengte pad
    [SerializeField] private float _minDistance; //minimum afstand tussen spawn en finish
    [SerializeField] private PathGrid _grid; //het grid
    [SerializeField] private PathFunctions _pathFnc; //functies voor het pad
    [SerializeField] private GameObject _player; //de speler
    [SerializeField] private GameObject _finish; //de finish
    private List<PathTile> _visitedTiles = new List<PathTile>(); //lijst met tiles van het pad

    public List<PathTile> VisitedTiles { get => _visitedTiles; set => _visitedTiles = value; }

    private void Awake() //grid en pad aanmaken
    {
        _grid.GenerateGrid();
        GeneratePath(SpawnPlayer());
    }

    public PathTile SpawnPlayer() //speler spawnen
    {
        List<PathTile> _possibleTiles = new List<PathTile>();

        //tiles van boven en onder zijkant toevoegen incl. tiles op hoeken
        for (int x = 0; x < _grid.Width; x++)
        {
            _possibleTiles.Add(_grid.GetTileAtPosition(new Vector2(x, 0)));
            _possibleTiles.Add(_grid.GetTileAtPosition(new Vector2(x, _grid.Height - 1)));
        }

        //tiles van linker en rechter zijkant toevoegen excl. tiles op hoeken
        for (int y = 1; y < _grid.Height - 1; y++)
        {
            _possibleTiles.Add(_grid.GetTileAtPosition(new Vector2(0, y)));
            _possibleTiles.Add(_grid.GetTileAtPosition(new Vector2(_grid.Width - 1, y)));
        }

        //een random tile kiezen voor de speler op te spawnen en de speler er op spawnen
        PathTile _playerSpawn = _possibleTiles[Random.Range(0, _possibleTiles.Count)];
        Player character = Instantiate(_player, _playerSpawn.transform.position, Quaternion.identity, _playerSpawn.transform).GetComponent<Player>();
        character.CurrentPositon = _grid.GetTilePosition(_playerSpawn);

        return _playerSpawn;
    }


    public void GeneratePath(PathTile spawnTile) //random pad genereren
    {
        int length = Random.Range(_minLength, Mathf.Min(_maxLength + 1, _grid.Width * _grid.Height / 2)); //random lengte kiezen en zorgen dat deze niet te lang is
        PathTile currentTile = spawnTile;
        bool reset = false;

        VisitedTiles.Add(currentTile);

        //pad genereren
        for (int i = 0; i < length; i++)
        {
            List<PathTile> _possibleTiles = _pathFnc.GetPossibleTiles(currentTile);

            //wanneer er geen mogelijke tiles meer zijn, dan is het pad gefaald en moet er gereset worden
            if (_possibleTiles.Count == 0)
            {
                reset = true;
                break;
            }

            //huidige tile instellen
            currentTile = _pathFnc.GetRandomTile(_possibleTiles, _grid.GetTilePosition(spawnTile));
            currentTile.SetTile(new Color(0f, 0f, 0f, 1) + new Color(i * 0.04f, i * 0.04f, i * 0.04f, 0), false, false, "");
            VisitedTiles.Add(currentTile);
        }

        //laatste tile instellen als finish
        currentTile.IsFinish = true;
        Instantiate(_finish, currentTile.transform.position, Quaternion.identity, currentTile.transform);

        //nakijken of er genoeg x en y afstand is tussen spawn en finish
        float distanceX = Mathf.Abs(spawnTile.transform.position.x - currentTile.transform.position.x);
        float distanceY = Mathf.Abs(spawnTile.transform.position.y - currentTile.transform.position.y);

        if (distanceX < _minDistance || distanceY < _minDistance)
        {
            reset = true;
        }


        //resetten wanneer gefaald, obstakels plaatsen bij succes
        if (reset)
        {
            Reset();
        }
        else
        {
            GenerateObstacles();
        }
    }

    public void GenerateObstacles() //obstakels plaatsen
    {
        List<PathTile> _possibleTiles = new List<PathTile>();

        //mogelijk tiles voor obstakels toevoegen in lijst
        for (int x = 0; x < _grid.Width; x++)
        {
            for (int y = 0; y < _grid.Height; y++)
            {
                PathTile currentTile = _grid.GetTileAtPosition(new Vector2(x, y));
                if (!VisitedTiles.Contains(currentTile)) //tile toevoegen in lijst wanneer deze zich niet op het pad bevind
                {
                    _possibleTiles.Add(currentTile);
                }
            }
        }

        //random mogelijke tiles obstakels van maken
        foreach (PathTile tile in _possibleTiles)
        {
            if (Random.value > 0.5f) //50% kans per tile
            {
                tile.SetTile(Color.yellow, true, false, "");
            }
        }
    }


    public void Reset() //wanneer het pad faalt om te genereren
    {
        Debug.Log("Generation fail");
        //lijsten leegmaken
        VisitedTiles.Clear();

        //opnieuw een pad maken
        _grid.GenerateGrid();
        GeneratePath(SpawnPlayer());
    }
}
