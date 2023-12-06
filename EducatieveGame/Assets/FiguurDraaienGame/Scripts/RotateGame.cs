using TMPro;
using UnityEngine;

public class RotateGame : MonoBehaviour
{
    [SerializeField] private GameObject _correctGrid; //object van het voorbeeld grid
    [SerializeField] private GameObject _gameGrid; //object van het game grid
    [SerializeField] private GameObject _checkFigure; //object van de controleerknop
    [SerializeField] private GameObject _statistics; //object van de statistieken
    [SerializeField] private TextMeshProUGUI _scoreText; //de text van de score
    [SerializeField] private Sprite _partSprite; //part sprite zonder rooster
    private GridGenerator _correctGridGen; //gridgenerator van het voorbeeldgrid
    private GridGenerator _gameGridGen; //gridgenerator van het gamegrid
    private bool _gameInProgress = true; //zorgen dat je geen vakjes meer kunt draaien als het spel gedaan is

    private GameObject CorrectGrid { get => _correctGrid; set => _correctGrid = value; }
    private GameObject GameGrid { get => _gameGrid; set => _gameGrid = value; }
    private GameObject CheckFigure { get => _checkFigure; set => _checkFigure = value; }
    private GameObject Statistics { get => _statistics; set => _statistics = value; }
    private TextMeshProUGUI ScoreText { get => _scoreText; set => _scoreText = value; }
    private Sprite PartSprite { get => _partSprite; set => _partSprite = value; }
    private GridGenerator CorrectGridGen { get => _correctGridGen; set => _correctGridGen = value; }
    private GridGenerator GameGridGen { get => _gameGridGen; set => _gameGridGen = value; }
    public bool GameInProgress { get => _gameInProgress; private set => _gameInProgress = value; }

    private void Awake() //grid instellen op moeilijkheid
    {
        CorrectGridGen = CorrectGrid.GetComponent<GridGenerator>();
        GameGridGen = GameGrid.GetComponent<GridGenerator>();

        //difficulty toepassen
        int difficulty = PlayerPrefs.GetInt("difficulty");

        switch (difficulty)
        {
            case 1:
                SetGridParameters(2, 4, 2, 8);
                break;

            case 2:
                SetGridParameters(4, 8, 2, 12);
                break;

            case 3:
                SetGridParameters(6, 12, 1, 7);
                break;

            case 4:
                SetGridParameters(8, 16, 1, 9);
                break;

            default:
                SetGridParameters(10, 20, 1, 11);
                break;
        }

        //grid aanmaken met parameters
        void SetGridParameters(int width, int height, int cellSize, int orthographicSizeOffset)
        {
            CorrectGridGen.GenerateGrid(width, height, cellSize);
            GameGridGen.GenerateGrid(width, height, cellSize);

            Vector3 camPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
            GameGrid.transform.position = camPosition;
            CorrectGrid.transform.position = new Vector3(camPosition.x - orthographicSizeOffset, camPosition.y, 0);
            Camera.main.orthographicSize = orthographicSizeOffset;
        }

        //figuurstukken van voorbeeldgrid een willekeurige rotatie geven
        for (int i = 0; i < CorrectGrid.transform.childCount; i++)
        {
            CorrectGrid.transform.GetChild(i).transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90.0f);
        }

        //configuratie rooster toepassen
        if (PlayerPrefs.GetInt("assist") == 1)
        {
            for (int i = 0; i < CorrectGrid.transform.childCount; i++)
            {
                CorrectGrid.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = PartSprite;
            }
            for (int i = 0; i < GameGrid.transform.childCount; i++)
            {
                GameGrid.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = PartSprite;
            }
        }
    }

    public void CheckGameGrid() //figuurstukken uit het voorbeeld grid vergelijken met het game grid, ze groen of rood kleuren en de score teruggeven
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
        GameInProgress = false;
        CheckFigure.SetActive(false);
        Statistics.SetActive(true);
    }
}
