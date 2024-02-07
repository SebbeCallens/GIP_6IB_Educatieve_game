using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class EndScreenLogic : MenuLogic
{
    [SerializeField] private Transform _difficultys;
    [SerializeField] private Transform _gameView;
    [SerializeField] private Transform _preview;
    [SerializeField] private Transform _gameStats;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _titleText;
    private static string _currentGame = "MainMenu";
    private static string _gameName = "Eindscherm";
    private static string _score = "0/0%";
    private static float _cameraSize = 5;
    private static float _offsetY = 5;

    private Transform Difficultys { get => _difficultys; set => _difficultys = value; }
    private Transform GameView { get => _gameView; set => _gameView = value; }
    private Transform Preview { get => _preview; set => _preview = value; }
    private Transform GameStats { get => _gameStats; set => _gameStats = value; }
    private TextMeshProUGUI ScoreText { get => _scoreText; set => _scoreText = value; }
    private TextMeshProUGUI TitleText { get => _titleText; set => _titleText = value; }
    private static string CurrentGame { get => _currentGame; set => _currentGame = value; }
    private static string GameName { get => _gameName; set => _gameName = value; }
    private static string Score { get => _score; set => _score = value; }
    private static float CameraSize { get => _cameraSize; set => _cameraSize = value; }
    private static float OffsetY { get => _offsetY; set => _offsetY = value; }

    private void Awake()
    {
        Difficultys.GetChild(Difficulty-1).gameObject.SetActive(true);
        ScoreText.text = Score;
        TitleText.text = GameName;
        GameObject gameView = GameObject.FindWithTag("GameView");
        if (gameView != null )
        {
            GameObject.FindWithTag("GameView").SetActive(true);
            GameObject.FindWithTag("GameView").transform.parent = GameView;
            GameObject.FindWithTag("GameView").transform.position = new(GameView.transform.position.x, GameView.transform.position.y - OffsetY, GameView.transform.position.z);
        }
        GameObject preview = GameObject.FindWithTag("Preview");
        if (preview != null)
        {
            GameObject.FindWithTag("Preview").SetActive(true);
            GameObject.FindWithTag("Preview").transform.SetParent(Preview);
            GameObject.FindWithTag("Preview").transform.position = new(Preview.transform.position.x, Preview.transform.position.y - OffsetY, Preview.transform.position.z);
        }
        Camera.main.orthographicSize = CameraSize;

        if (CurrentGame.Equals("RotateFigure"))
        {
            GameStats.GetChild(0).gameObject.SetActive(true);
            if (PlayerPrefs.GetInt("assist") == 1)
            {
                GameStats.GetChild(0).GetChild(0).GetChild(0).GetComponent<Toggle>().isOn = true;
            }
            if (PlayerPrefs.GetInt("symmetrical") == 1)
            {
                GameStats.GetChild(0).GetChild(1).GetChild(0).GetComponent<Toggle>().isOn = true;
            }
        }
        else if (CurrentGame.Equals("SelectDrawMode"))
        {
            GameStats.GetChild(1).gameObject.SetActive(true);
            if (PlayerPrefs.GetInt("figure-assist") == 1)
            {
                GameStats.GetChild(1).GetChild(0).GetChild(0).GetComponent<Toggle>().isOn = true;
            }
            LineRenderer lineRend = GameView.GetChild(0).GetComponent<LineRenderer>();
            for (int i = 0; i < lineRend.positionCount; i++)
            {
                Vector3 position = lineRend.GetPosition(i);
                position.x -= 40;
                position.y -= 40 + OffsetY;
                lineRend.SetPosition(i, position);
            } 
        }
        else if (CurrentGame.Equals("ReactionMenu"))
        {
            GameStats.GetChild(2).gameObject.SetActive(true);
            Difficultys.GetChild(Difficulty - 1).GetChild(0).GetChild(PlayerPrefs.GetInt("meat")-1).gameObject.SetActive(true);
            Difficultys.GetChild(Difficulty - 1).GetChild(1).GetChild(5-PlayerPrefs.GetInt("rate")).gameObject.SetActive(true);
            Difficultys.GetChild(Difficulty - 1).GetChild(2).GetChild(PlayerPrefs.GetInt("size")-1).gameObject.SetActive(true);
        }
        else if (CurrentGame.Equals("PuzzelGameMenu"))
        {
            GameStats.GetChild(3).gameObject.SetActive(true);

        }
        else if (CurrentGame.Equals("Startscherm"))
        {
            GameStats.GetChild(4).gameObject.SetActive(true);

        }
        else if (CurrentGame.Equals("PadVolgenMenu"))
        {
            GameStats.GetChild(5).gameObject.SetActive(true);

        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(CurrentGame);
    }

    public static void EndGame(string sceneName, string gameName, string score, float cameraSize, float offsetY)
    {
        CurrentGame = sceneName;
        GameName = gameName;
        Score = score;
        CameraSize = cameraSize;
        OffsetY = OffsetY;
    }
}
