using TMPro;
using UnityEngine;

public class RotateGame : MonoBehaviour
{
    [SerializeField] private GameObject _correctGrid;
    [SerializeField] private GameObject _gameGrid;
    [SerializeField] private GameObject _checkFigure;
    [SerializeField] private GameObject _statistics;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Camera _cam;
    private GridGenerator _correctGridGen;
    private GridGenerator _gameGridGen;
    private GridFunctions _correctGridFunc;
    private GridFunctions _gameGridFunc;
    private int _width;
    private int _height;
    private int _cellSize;

    private GameObject CorrectGrid { get => _correctGrid; set => _correctGrid = value; }
    private GameObject GameGrid { get => _gameGrid; set => _gameGrid = value; }
    private GameObject CheckFigure { get => _checkFigure; set => _checkFigure = value; }
    private GameObject Statistics { get => _statistics; set => _statistics = value; }
    private TextMeshProUGUI ScoreText { get => _scoreText; set => _scoreText = value; }
    private Camera Cam { get => _cam; set => _cam = value; }
    private GridGenerator CorrectGridGen { get => _correctGridGen; set => _correctGridGen = value; }
    private GridGenerator GameGridGen { get => _gameGridGen; set => _gameGridGen = value; }
    private GridFunctions CorrectGridFunc { get => _correctGridFunc; set => _correctGridFunc = value; }
    private GridFunctions GameGridFunc { get => _gameGridFunc; set => _gameGridFunc = value; }
    private int Width { get => _width; set => _width = value; }
    private int Height { get => _height; set => _height = value; }
    public int CellSize { get => _cellSize; private set => _cellSize = value; }

    private void Awake()
    {
        CorrectGridGen = CorrectGrid.GetComponent<GridGenerator>();
        CorrectGridFunc = CorrectGrid.GetComponent<GridFunctions>();
        GameGridGen = GameGrid.GetComponent<GridGenerator>();
        GameGridFunc = GameGrid.GetComponent<GridFunctions>();

        if (PlayerPrefs.GetInt("difficulty") == 5)
        {
            Width = 2;
            Height = 4;
            CellSize = 2;
            GameGrid.transform.position = new(Cam.transform.position.x, Cam.transform.position.y, 0);
            CorrectGrid.transform.position = new(Cam.transform.position.x - 8, Cam.transform.position.y, 0);
            Cam.orthographicSize = 8;
        }
        else if (PlayerPrefs.GetInt("difficulty") == 4)
        {
            Width = 4;
            Height = 8;
            CellSize = 2;
            GameGrid.transform.position = new(Cam.transform.position.x, Cam.transform.position.y, 0);
            CorrectGrid.transform.position = new(Cam.transform.position.x - 12, Cam.transform.position.y, 0);
            Cam.orthographicSize = 12;
        }
        else if (PlayerPrefs.GetInt("difficulty") == 3)
        {
            Width = 6;
            Height = 12;
            CellSize = 1;
            GameGrid.transform.position = new(Cam.transform.position.x, Cam.transform.position.y, 0);
            CorrectGrid.transform.position = new(Cam.transform.position.x - 8, Cam.transform.position.y, 0);
            Cam.orthographicSize = 7;
        }
        else if (PlayerPrefs.GetInt("difficulty") == 2)
        {
            Width = 8;
            Height = 16;
            CellSize = 1;
            GameGrid.transform.position = new(Cam.transform.position.x, Cam.transform.position.y, 0);
            CorrectGrid.transform.position = new(Cam.transform.position.x - 10, Cam.transform.position.y, 0);
            Cam.orthographicSize = 9;
        }
        else
        {
            Width = 10;
            Height = 20;
            CellSize = 1;
            GameGrid.transform.position = new(Cam.transform.position.x, Cam.transform.position.y, 0);
            CorrectGrid.transform.position = new(Cam.transform.position.x - 12, Cam.transform.position.y, 0);
            Cam.orthographicSize = 11;
        }

        CorrectGridGen.GenerateGrid(Width, Height, CellSize);
        GameGridGen.GenerateGrid(Width, Height, CellSize);

        for (int i = 0; i < CorrectGrid.transform.childCount; i++)
        {
            CorrectGrid.transform.GetChild(i).transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90.0f);
        }
    }

    public void CheckGameGrid()
    {
        int correctCells = 0;
        for (int i = 0; i < CorrectGrid.transform.childCount; i++)
        {
            if (CorrectGrid.transform.GetChild(i).transform.rotation.eulerAngles.z == GameGrid.transform.GetChild(i).transform.rotation.eulerAngles.z)
            {
                GameGrid.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.green;
                correctCells++;
            }
            else
            {
                GameGrid.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        ScoreText.text = "Score: " + correctCells + "/" + CorrectGrid.transform.childCount;
        CheckFigure.SetActive(false);
        Statistics.SetActive(true);
    }
}
