using System.Collections.Generic;
using TMPro;
using UnityEngine;

//dit script zorgt voor een pad in het grid
public class PathGenerator : MonoBehaviour
{
    [SerializeField] private int _minLength; //minimum lengte pad
    [SerializeField] private int _maxLength; //maximum lengte pad
    [SerializeField] private float _minDistance; //minimum afstand tussen spawn en finish
    [SerializeField] private bool _randomOrder; //of het pad in willekeurige volgorde mag gevolgd worden
    [SerializeField] private bool _arrows; //pijl aanduiding
    [SerializeField] private bool _scrambledOrder; //pad checkpoints scrambled
    [SerializeField] private PathManager _pad; //het pad
    [SerializeField] private GameObject _player; //de speler
    [SerializeField] private GameObject _finish; //de finish
    [SerializeField] private TextMeshProUGUI _tileText; //text voor locatie nummer
    [SerializeField] private Transform _canvas; //canvas in de scene
    [SerializeField] private GameObject _location;
    [SerializeField] private GameObject _obstacle;
    private int _paths = 0; //hoeveel paden er geprobeerd zijn

    private int MinLength { get => _minLength; set => _minLength = value; }
    private int MaxLength { get => _maxLength; set => _maxLength = value; }
    private float MinDistance { get => _minDistance; set => _minDistance = value; }
    public bool RandomOrder { get => _randomOrder; private set => _randomOrder = value; }
    public bool Arrows { get => _arrows; private set => _arrows = value; }
    public bool ScrambledOrder { get => _scrambledOrder; private set => _scrambledOrder = value; }
    private PathManager Pad { get => _pad; set => _pad = value; }
    private GameObject Player { get => _player; set => _player = value; }
    private GameObject Finish { get => _finish; set => _finish = value; }
    public TextMeshProUGUI TileText { get => _tileText; private set => _tileText = value; }
    public Transform Canvas { get => _canvas; private set => _canvas = value; }
    private int Paths { get => _paths; set => _paths = value; }

    private void Awake()
    {
        if (PlayerPrefs.GetInt("randomorder") == 1)
        {
            ScrambledOrder = true;
        }
        else
        {
            ScrambledOrder = false;
        }
        if (PlayerPrefs.GetInt("chooseorder") == 1)
        {
            RandomOrder = true;
        }
        else
        {
            RandomOrder = false;
        }
        if (PlayerPrefs.GetInt("pad-assist") == 1)
        {
            Arrows = true;
        }
        else
        {
            Arrows = false;
        }
        MinLength = MenuLogic.Difficulty * 8;
        MaxLength = MenuLogic.Difficulty * 10;
        MinDistance = MenuLogic.Difficulty;
    }

    public PathTile SpawnPlayer() //speler spawnen
    {
        List<PathTile> _possibleTiles = new List<PathTile>();

        //tiles van boven en onder zijkant toevoegen incl. tiles op hoeken
        for (int x = 0; x < Pad.Grid.Width; x++)
        {
            _possibleTiles.Add(Pad.Grid.GetTileAtPosition(new Vector2(x, 0)));
            _possibleTiles.Add(Pad.Grid.GetTileAtPosition(new Vector2(x, Pad.Grid.Height - 1)));
        }

        //tiles van linker en rechter zijkant toevoegen excl. tiles op hoeken
        for (int y = 1; y < Pad.Grid.Height - 1; y++)
        {
            _possibleTiles.Add(Pad.Grid.GetTileAtPosition(new Vector2(0, y)));
            _possibleTiles.Add(Pad.Grid.GetTileAtPosition(new Vector2(Pad.Grid.Width - 1, y)));
        }

        //een random tile kiezen voor de speler op te spawnen en de speler er op spawnen
        PathTile playerSpawn = _possibleTiles[Random.Range(0, _possibleTiles.Count)];
        Pad.Checkpoints.Add(playerSpawn);

        return playerSpawn;
    }


    public void GeneratePath(PathTile spawnTile, int locations) //random pad genereren
    {
        Paths++;

        int length = Random.Range(MinLength, Mathf.Min(MaxLength + 1, Pad.Grid.Width * Pad.Grid.Height / 2)); //random lengte kiezen en zorgen dat deze niet te lang is
        PathTile currentTile = spawnTile;
        bool reset = false;
        int interval = length / (locations + 1);
        int locationCount = 0;

        //pad genereren
        for (int i = 0; i < length - 1; i++)
        {
            List<PathTile> _possibleTiles = Pad.Functions.GetPossibleTiles(currentTile);

            //wanneer er geen mogelijke tiles meer zijn, dan is het pad gefaald en moet er gereset worden
            if (_possibleTiles.Count == 0)
            {
                reset = true;
                break;
            }

            if (i % interval == 0 && i > 1 && locationCount < locations) //tile op index 10 wordt nu altijd een locatie
            {
                locationCount++;
                //huidige tile instellen
                currentTile = Pad.Functions.GetRandomTile(_possibleTiles, Pad.Grid.GetTilePosition(spawnTile));
                currentTile.SetTile(Color.white, false, true);
                Instantiate(_location, currentTile.transform.position, Quaternion.identity, currentTile.transform);
                if (!RandomOrder)
                {
                    Pad.Checkpoints.Add(currentTile);
                    currentTile.GetComponentInChildren<TextMeshPro>().text = locationCount.ToString();
                }
            }
            else
            {
                //huidige tile instellen
                currentTile = Pad.Functions.GetRandomTile(_possibleTiles, Pad.Grid.GetTilePosition(spawnTile));
                currentTile.SetTile(Color.white, false, false);
            }
            Pad.RandomPath.Add(currentTile);
        }

        //laatste tile instellen als finish
        currentTile.IsFinish = true;
        Instantiate(Finish, currentTile.transform.position, Quaternion.identity, currentTile.transform);
        Pad.Checkpoints.Add(currentTile);

        //nakijken of er genoeg afstand is tussen spawn en finish
        float distance = Mathf.Sqrt(Mathf.Pow(spawnTile.transform.position.x - currentTile.transform.position.x, 2) + Mathf.Pow(spawnTile.transform.position.y - currentTile.transform.position.y, 2));

        if (distance < MinDistance)
        {
            reset = true;
        }

        //resetten wanneer gefaald, obstakels plaatsen bij succes
        if (reset)
        {
            Reset(locations);
        }
        else
        {
            GenerateObstacles(spawnTile);
        }
    }

    public void GenerateObstacles(PathTile spawnTile) //obstakels plaatsen
    {
        List<PathTile> possibleTiles = new List<PathTile>();
        Player character = Instantiate(Player, spawnTile.transform.position, Quaternion.identity, spawnTile.transform).GetComponent<Player>();
        character.CurrentPosition = Pad.Grid.GetTilePosition(spawnTile);

        //mogelijk tiles voor obstakels toevoegen in lijst
        for (int x = 0; x < Pad.Grid.Width; x++)
        {
            for (int y = 0; y < Pad.Grid.Height; y++)
            {
                PathTile currentTile = Pad.Grid.GetTileAtPosition(new Vector2(x, y));
                if (!Pad.RandomPath.Contains(currentTile) && !Pad.Checkpoints.Contains(currentTile)) //tile toevoegen in lijst wanneer deze zich niet op het pad bevind
                {
                    possibleTiles.Add(currentTile);
                }
            }
        }

        //random mogelijke tiles obstakels van maken
        foreach (PathTile tile in possibleTiles)
        {
            if (Random.value > 0.75f) //25% kans per tile
            {
                tile.SetTile(Color.white, true, false);
                Instantiate(_obstacle, tile.transform.position, Quaternion.identity, tile.transform);
            }
        }

        if (!RandomOrder)
        {
            if (ScrambledOrder)
            {
                ScrambleLocations();
                FindShortestPath();
            }
            if (Arrows)
            {
                Pad.Finder.ShowArrow();
            }
        }
        else
        {
            ScrambledOrder = false;
            Arrows = false;
        }
    }

    private void ScrambleLocations()
    {
        for (int i = 0; i < Canvas.transform.childCount; i++)
        {
            Destroy(Canvas.transform.GetChild(i).gameObject);
        }

        PathTile firstCheckpoint = Pad.Checkpoints[0];
        PathTile lastCheckpoint = Pad.Checkpoints[Pad.Checkpoints.Count - 1];

        List<PathTile> intermediateList = new List<PathTile>(Pad.Checkpoints.GetRange(1, Pad.Checkpoints.Count - 2));

        for (int i = 0; i < intermediateList.Count; i++)
        {
            int randomIndex = Random.Range(i, intermediateList.Count);
            PathTile temp = intermediateList[i];
            intermediateList[i] = intermediateList[randomIndex];
            intermediateList[randomIndex] = temp;
        }

        intermediateList.Insert(0, firstCheckpoint);
        intermediateList.Add(lastCheckpoint);

        Pad.Checkpoints = intermediateList;

        for (int i = 1; i < Pad.Checkpoints.Count - 1; i++)
        {
            Pad.Checkpoints[i].GetComponentInChildren<TextMeshPro>().text = i.ToString();
        }
    }

    public void FindShortestPath() //kortste pad zoeken
    {
        for (int i = 0; i < Pad.Checkpoints.Count - 1; i++)
        {
            List<PathTile> shortestPath = Pad.AStar.FindShortestPath(Pad.Checkpoints[i], Pad.Checkpoints[i + 1]);
            shortestPath.RemoveAt(0);
            Pad.AStarPath.AddRange(shortestPath);
        }
    }

    public void ShowAStarPath(Color color)
    {
        if (!ScrambledOrder)
        {
            FindShortestPath();
        }

        foreach (PathTile tile in Pad.AStarPath)
        {
            float scaleFactor = 0.1f + (0.8f * Pad.AStarPath.IndexOf(tile) / (float)(Pad.AStarPath.Count - 1));
            tile.PathHighlight.GetComponent<SpriteRenderer>().color = color;
            tile.PathHighlight.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        }
    }

    public void Reset(int locations) //wanneer het pad faalt om te genereren
    {
        Debug.Log("Generation fail");

        for (int i = 0; i < Canvas.transform.childCount; i++)
        {
            Destroy(Canvas.transform.GetChild(i).gameObject);
        }

        if (Paths >= 100) //maxium 100 paden proberen voor oneindige loop tegen te gaan
        {
            Debug.Log("Maximum tries exceeded");
        }
        else
        {
            //lijsten leegmaken
            Pad.RandomPath.Clear();
            Pad.Checkpoints.Clear();

            //opnieuw een pad maken
            Pad.Grid.GenerateGrid();
            GeneratePath(SpawnPlayer(), locations);
        }
    }
}
