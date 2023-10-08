using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _meat; //de prefab voor een vlees object
    [SerializeField] private GameObject _statistics; //de statistieken
    [SerializeField] private int _minutesUntilFastest; //hoeveel minuten tot de snelste spawnrate:
    [SerializeField] private Camera _cam; //de camera van de scene
    private GameObject[] _gridCells; //lijst met de gridcellen
    private GridGenerator _gridGen; //de gridgenerator
    private GridFunctions _gridFunc; //de gridfuncties
    private Vector3[] _gridPoints; //lijst met de gridpunten
    private bool _gameActive = true; //of het spel bezig is of al gedaan is
    private float _spawnRate; //hoe vlug er vlees spawned op de barbecue
    private float _lastSpawnTime; //de laatste tijd wanneer er vlees gespawned is
    private float _lastDecreaseTime; //de laatste tijd wanneer de moeilijkheid hoger gezet is
    private float _difficulty; //moeilijkheid van het spel
    private int _cellSize; //instellingen grid
    private int _width; //instellingen grid
    private int _height; //instellingen grid
    private GameObject Meat { get => _meat; set => _meat = value; }
    private GameObject Statistics { get => _statistics; set => _statistics = value; }
    private int MinutesUntilFastest { get => _minutesUntilFastest; set => _minutesUntilFastest = value; }
    private Camera Cam { get => _cam; set => _cam = value; }
    private GameObject[] GridCells { get => _gridCells; set => _gridCells = value; }
    private GridGenerator GridGen { get => _gridGen; set => _gridGen = value; }
    private GridFunctions GridFunc { get => _gridFunc; set => _gridFunc = value; }
    private Vector3[] GridPoints { get => _gridPoints; set => _gridPoints = value; }
    private bool GameActive { get => _gameActive; set => _gameActive = value; }
    private float SpawnRate { get => _spawnRate; set => _spawnRate = value; }
    private float LastSpawnTime { get => _lastSpawnTime; set => _lastSpawnTime = value; }
    private float LastDecreaseTime { get => _lastDecreaseTime; set => _lastDecreaseTime = value; }
    private float Difficulty { get => _difficulty; set => _difficulty = value; }
    public int CellSize { get => _cellSize; private set => _cellSize = value; }
    private int Width { get => _width; set => _width = value; }
    private int Height { get => _height; set => _height = value; }

    private void Awake() //grid genereren
    {
        GridGen = GetComponent<GridGenerator>();
        GridFunc = GetComponent<GridFunctions>();
        Difficulty = PlayerPrefs.GetInt("difficulty");

        if (Difficulty == 5)
        {
            CellSize = 2;
            Width = 4;
            Height = 4;
            Cam.orthographicSize = 6;
        }
        else if (Difficulty == 4)
        {
            CellSize = 2;
            Width = 5;
            Height = 5;
            Cam.orthographicSize = 7.5f;
        }
        else if (Difficulty == 3)
        {
            CellSize = 1;
            Width = 6;
            Height = 6;
            Cam.orthographicSize = 4.5f;
        }
        else if (Difficulty == 2)
        {
            CellSize = 1;
            Width = 7;
            Height = 7;
            Cam.orthographicSize = 5.25f;
        }
        else
        {
            CellSize = 1;
            Width = 8;
            Height = 8;
            Cam.orthographicSize = 6;
        }

        GridGen.GenerateGrid(Width, Height, CellSize);
        GridPoints = GridFunc.CenterGridPoints(CellSize);
        GridCells = new GameObject[transform.childCount];
        SpawnRate = Difficulty;

        for (int i = 0; i < transform.childCount; i++)
        {
            GridCells[i] = transform.GetChild(i).gameObject;
        }

        LastSpawnTime = Time.time;
        LastDecreaseTime = Time.time;
    }

    private void Update() //vlees spawnen op de barbecue
    {
        if (Time.time - LastSpawnTime > SpawnRate && GameActive)
        {
            bool meatSpawned = false;
            int i = 0;
            foreach (Vector3 gridPoint in GridPoints)
            {
                if (Random.value >= 1 - 1 / (float)GridPoints.Length && !meatSpawned && GridCells[i].transform.childCount == 0)
                {
                    Instantiate(Meat, new Vector3(gridPoint.x + 0.08f, gridPoint.y - 0.08f, gridPoint.z), Quaternion.identity, GridCells[i].transform);
                    meatSpawned = true;
                }
                i++;
            }

            LastSpawnTime = Time.time;
        }

        if (SpawnRate > 0.4f * Difficulty)
        {
            if (Time.time - LastDecreaseTime > 5 * MinutesUntilFastest)
            {
                SpawnRate -= 0.05f * Difficulty;
                LastDecreaseTime = Time.time;
            }
        }
        else if (Time.time - LastDecreaseTime > 60 * MinutesUntilFastest - Difficulty * 2 - 1 && GameActive)
        {
            GameActive = false;
        }
        else if (Time.time - LastDecreaseTime > 60 * MinutesUntilFastest)
        {
            Statistics.SetActive(true);
        }
    }
}
