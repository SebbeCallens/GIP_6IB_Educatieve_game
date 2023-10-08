using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _meat; //de prefab voor een vlees object
    [SerializeField] private int _cellSize; //later laten instellen door gebruiker
    [SerializeField] private int _width; //later laten instellen door moeilijkheidsgraad
    [SerializeField] private int _height; //later laten instellen door moeilijkheidsgraad
    [SerializeField] private int _minutesUntilFastest; //hoeveel minuten tot de snelste spawnrate
    private GameObject[] _gridCells; //lijst met de gridcellen
    private GridGenerator _gridGen; //de gridgenerator
    private GridFunctions _gridFunc; //de gridfuncties
    private Vector3[] _gridPoints; //lijst met de gridpunten
    private float _spawnRate; //hoe vlug er vlees spawned op de barbecue
    private float _lastSpawnTime; //de laatste tijd wanneer er vlees gespawned is
    private float _lastDecreaseTime; //de laatste tijd wanneer de moeilijkheid hoger gezet is
    private float _difficulty; //moeilijkheid van het spel

    private GameObject Meat { get => _meat; set => _meat = value; }
    public int CellSize { get => _cellSize; private set => _cellSize = value; }
    private int Width { get => _width; set => _width = value; }
    private int Height { get => _height; set => _height = value; }
    private int MinutesUntilFastest { get => _minutesUntilFastest; set => _minutesUntilFastest = value; }
    private GameObject[] GridCells { get => _gridCells; set => _gridCells = value; }
    private GridGenerator GridGen { get => _gridGen; set => _gridGen = value; }
    private GridFunctions GridFunc { get => _gridFunc; set => _gridFunc = value; }
    private Vector3[] GridPoints { get => _gridPoints; set => _gridPoints = value; }
    private float SpawnRate { get => _spawnRate; set => _spawnRate = value; }
    private float LastSpawnTime { get => _lastSpawnTime; set => _lastSpawnTime = value; }
    private float LastDecreaseTime { get => _lastDecreaseTime; set => _lastDecreaseTime = value; }
    private float Difficulty { get => _difficulty; set => _difficulty = value; }

    private void Awake() //grid genereren
    {
        GridGen = GetComponent<GridGenerator>();
        GridFunc = GetComponent<GridFunctions>();
        GridGen.GenerateGrid(Width / CellSize, Height / CellSize, CellSize);
        GridPoints = GridFunc.CenterGridPoints(CellSize);
        GridCells = new GameObject[transform.childCount];
        Difficulty = PlayerPrefs.GetInt("difficulty");
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
        if (Time.time - LastSpawnTime > SpawnRate)
        {
            bool meatSpawned = false;
            int i = 0;
            foreach (Vector3 gridPoint in GridPoints)
            {
                if (Random.value >= 1 - 1 / (float)GridPoints.Length && !meatSpawned && GridCells[i].transform.childCount == 0)
                {
                    Instantiate(Meat, gridPoint, Quaternion.identity, GridCells[i].transform);
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
    }
}
