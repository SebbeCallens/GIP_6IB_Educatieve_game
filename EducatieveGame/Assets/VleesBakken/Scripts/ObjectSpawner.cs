using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _meat; //de prefab voor een vlees object
    [SerializeField] private GameObject _statistics; //de statistieken
    [SerializeField] private int _minutesUntilFastest; //hoeveel minuten tot de snelste spawnrate:
    private GameObject[] _gridCells; //lijst met de gridcellen
    private GridGenerator _gridGen; //de gridgenerator
    private GridFunctions _gridFunc; //de gridfuncties
    [SerializeField] private Stats _statsObj;
    private Vector3[] _gridPoints; //lijst met de gridpunten
    private bool _gameActive = true; //of het spel bezig is of al gedaan is
    private float _spawnRate; //hoe vlug er vlees spawned op de barbecue
    private float _lastSpawnTime; //de laatste tijd wanneer er vlees gespawned is
    private float _lastDecreaseTime; //de laatste tijd wanneer de moeilijkheid hoger gezet is
    private float _difficulty; //moeilijkheid van het spel

    private GameObject Meat { get => _meat; set => _meat = value; }
    private GameObject Statistics { get => _statistics; set => _statistics = value; }
    private int MinutesUntilFastest { get => _minutesUntilFastest; set => _minutesUntilFastest = value; }
    private GameObject[] GridCells { get => _gridCells; set => _gridCells = value; }
    private GridGenerator GridGen { get => _gridGen; set => _gridGen = value; }
    private GridFunctions GridFunc { get => _gridFunc; set => _gridFunc = value; }
    private Stats StatsObj { get => _statsObj; set => _statsObj = value; }
    private Vector3[] GridPoints { get => _gridPoints; set => _gridPoints = value; }
    private bool GameActive { get => _gameActive; set => _gameActive = value; }
    private float SpawnRate { get => _spawnRate; set => _spawnRate = value; }
    private float LastSpawnTime { get => _lastSpawnTime; set => _lastSpawnTime = value; }
    private float LastDecreaseTime { get => _lastDecreaseTime; set => _lastDecreaseTime = value; }
    private float Difficulty { get => _difficulty; set => _difficulty = value; }

    private void Awake() //grid genereren
    {
        GridGen = GetComponent<GridGenerator>();
        GridFunc = GetComponent<GridFunctions>();
        Difficulty = PlayerPrefs.GetInt("rate");
        int gridSize = PlayerPrefs.GetInt("size");

        switch (gridSize)
        {
            case 1:
                SetGridProperties(2, 4, 4, 6);
                break;
            case 2:
                SetGridProperties(2, 5, 5, 7.5f);
                break;
            case 3:
                SetGridProperties(1, 6, 6, 4.5f);
                break;
            case 4:
                SetGridProperties(1, 7, 7, 5.25f);
                break;
            default:
                SetGridProperties(1, 8, 8, 6);
                break;
        }

        void SetGridProperties(int cellSize, int width, int height, float orthographicSize)
        {
            GridGen.GenerateGrid(width, height, cellSize);
            GridPoints = GridFunc.CenterGridPoints(cellSize);
            Camera.main.orthographicSize = orthographicSize;
        }

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
        if (Time.time - LastSpawnTime > SpawnRate / 1.5f && GameActive)
        {
            bool meatSpawned = false;
            int i = 0;
            foreach (Vector3 gridPoint in GridPoints)
            {
                if (Random.value >= 1 - 1 / (float)GridPoints.Length && !meatSpawned && GridCells[i].transform.childCount == 0)
                {
                    Instantiate(Meat, new Vector3(gridPoint.x + 0.032f, gridPoint.y - 0.032f, gridPoint.z), Quaternion.identity, GridCells[i].transform);
                    meatSpawned = true;
                }
                i++;
            }

            LastSpawnTime = Time.time;
        }

        if (SpawnRate < 0.4f * Difficulty)
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

    public void EndGame()
    {
        string score = string.Empty;
        for (int i = 0; i < StatsObj.StatNames.Length; i++)
        {
            if (i == 0)
            {
                score += $"{StatsObj.StatValues[i]}\n";
                score += "Aantal stukken vlees:\n";
            }
            else if (i == 1)
            {
                score += $"{StatsObj.StatValues[i]} {StatsObj.StatNames[i]}: {StatsObj.StatValues[i] * 1}\n";
            }
            else if (i == 2)
            {
                score += $"{StatsObj.StatValues[i]} {StatsObj.StatNames[i]}: {StatsObj.StatValues[i] * -1}\n";
            }
            else if (i == 3)
            {
                score += $"{StatsObj.StatValues[i]} {StatsObj.StatNames[i]}: {StatsObj.StatValues[i] * -2}\n";
            }
            else if (i == 4)
            {
                score += $"{StatsObj.StatValues[i]} {StatsObj.StatNames[i]}: {StatsObj.StatValues[i] * -1}\n";
            }
            else if (i == 5)
            {
                score += $"{StatsObj.StatValues[i]} {StatsObj.StatNames[i]}: {StatsObj.StatValues[i] * -3}\n";
            }
        }
        EndScreenLogic.EndGame("ReactionMenu", "Reactie game", score, 6, 5, 0);
        SceneManager.LoadScene("EndScreen");
    }
}
