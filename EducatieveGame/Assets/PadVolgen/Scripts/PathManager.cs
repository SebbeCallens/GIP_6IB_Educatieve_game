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
    [SerializeField] private PathAStar _aStar; //script A Star
    private List<PathTile> _visitedTiles = new List<PathTile>(); //lijst met tiles van het pad
    private int _paths = 0; //hoeveel paden er geprobeerd zijn
    private List<PathTile> _locations = new List<PathTile>(); //tiles start, locaties en einde

    public List<PathTile> VisitedTiles { get => _visitedTiles; set => _visitedTiles = value; }

    private void Start() //grid en pad aanmaken
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
        PathTile playerSpawn = _possibleTiles[Random.Range(0, _possibleTiles.Count)];
        Player character = Instantiate(_player, playerSpawn.transform.position, Quaternion.identity, playerSpawn.transform).GetComponent<Player>();
        character.CurrentPositon = _grid.GetTilePosition(playerSpawn);
        _locations.Add(playerSpawn);

        return playerSpawn;
    }


    public void GeneratePath(PathTile spawnTile) //random pad genereren
    {
        _paths++;

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

            if (i == 10) //tile op index 10 wordt nu altijd een locatie
            {
                //huidige tile instellen
                currentTile = _pathFnc.GetRandomTile(_possibleTiles, _grid.GetTilePosition(spawnTile));
                currentTile.SetTile(Color.grey, false, true, "random");
                VisitedTiles.Add(currentTile);
                _locations.Add(currentTile);
            }
            else
            {
                //huidige tile instellen
                currentTile = _pathFnc.GetRandomTile(_possibleTiles, _grid.GetTilePosition(spawnTile));
                currentTile.SetTile(Color.red, false, false, "");
                VisitedTiles.Add(currentTile);
            }
        }

        //laatste tile instellen als finish
        currentTile.IsFinish = true;
        Instantiate(_finish, currentTile.transform.position, Quaternion.identity, currentTile.transform);
        _locations.Add(currentTile);

        //nakijken of er genoeg afstand is tussen spawn en finish
        float distance = Mathf.Sqrt(Mathf.Pow(spawnTile.transform.position.x - currentTile.transform.position.x, 2)
                                   + Mathf.Pow(spawnTile.transform.position.y - currentTile.transform.position.y, 2));

        if (distance < _minDistance)
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
            if (Random.value > 0.75f) //25% kans per tile
            {
                tile.SetTile(Color.yellow, true, false, "");
            }
        }

        FindShortestPath();
    }

    public void FindShortestPath() //kortste pad zoeken
    {
        for (int i = 0; i < _locations.Count - 1; i++)
        {
            List<PathTile> shortestPath = _aStar.FindShortestPath(_locations[i], _locations[i + 1]);

            if (shortestPath != null)
            {
                foreach (PathTile tile in shortestPath)
                {
                    if (!tile.IsLocation && !VisitedTiles.Contains(tile))
                    {
                        tile.SetTile(Color.blue, false, false, "");
                    }
                    else if (!tile.IsLocation && VisitedTiles.Contains(tile))
                    {
                        tile.SetTile(Color.cyan, false, false, "");
                    }
                }
            }
            else
            {
                Debug.Log("A star error");
            }
        }
    }


    public void Reset() //wanneer het pad faalt om te genereren
    {
        Debug.Log("Generation fail");

        if (_paths >= 100) //maxium 100 paden proberen voor oneindige loop tegen te gaan
        {
            Debug.Log("maximum tries exceeded");
        }
        else
        {
            //lijsten leegmaken
            VisitedTiles.Clear();
            _locations.Clear();

            //opnieuw een pad maken
            _grid.GenerateGrid();
            GeneratePath(SpawnPlayer());
        }
    }
}
