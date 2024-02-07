using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private Slicer _puzzleSlicer;
    [SerializeField] private GameObject _puzzleSlot;
    [SerializeField] private GameObject _cornerSlot;
    [SerializeField] private GameObject _coordinateSlot;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _coordinates;
    private bool _extraChance = true;
    private List<GameObject> _puzzleSlots;

    private Slicer PuzzleSlicer { get => _puzzleSlicer; set => _puzzleSlicer = value; }
    private GameObject PuzzleSlot { get => _puzzleSlot; set => _puzzleSlot = value; }
    private GameObject CornerSlot { get => _cornerSlot; set => _cornerSlot = value; }
    private GameObject CoordinateSlot { get => _coordinateSlot; set => _coordinateSlot = value; }
    private TextMeshProUGUI ScoreText { get => _scoreText; set => _scoreText = value; }
    private GameObject Coordinates { get => _coordinates; set => _coordinates = value; }
    private List<GameObject> PuzzleSlots { get => _puzzleSlots; set => _puzzleSlots = value; }

    void Awake()
    {
        int difficulty = PlayerPrefs.GetInt("puzzeldifficulty");

        //hier kan je nog scale, width, height en de afbeelding ophalen
        (int columns, int rows, float gridScale) = PuzzleSlicer.SliceImage(PuzzleMenu.PuzzleImage.texture, difficulty * 3, difficulty * 3); //slice afbeelding met scale, aantal kolommen en aantal rijen

        //lijst van de de stukjes
        List<GameObject> parts = new();

        //lijsten instellen
        for (int i = 0; i < PuzzleSlicer.transform.GetChild(0).childCount; i++)
        {
            parts.Add(PuzzleSlicer.transform.GetChild(0).GetChild(i).gameObject);
        }

        //posities stukjes randomiseren
        for (int i = parts.Count - 1; i >= 0; i--)
        {
            int randomIndex = Random.Range(0, parts.Count);
            parts[randomIndex].transform.SetAsFirstSibling();
            parts.RemoveAt(randomIndex);
        }

        transform.GetChild(0).GetComponent<GridLayoutGroup>().constraintCount = columns + 1;
        PuzzleSlicer.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize *= gridScale;
        transform.GetChild(0).transform.localScale = new(gridScale, gridScale, 1);

        PuzzleSlots = new();

        //voor elke rij
        for (int row = -1; row < rows; row++)
        {
            //voor elke kolom
            for (int col = -1; col < columns; col++)
            {
                if (col == -1 && row == -1)
                {
                    Instantiate(CornerSlot, Vector3.zero, Quaternion.identity, transform.GetChild(0).transform);
                }
                else if (row == -1)
                {
                    TextMeshProUGUI coords = Instantiate(CoordinateSlot, Vector3.zero, Quaternion.identity, transform.GetChild(0).transform).GetComponentInChildren<TextMeshProUGUI>();
                    coords.text = PuzzleSlicer.IntToChar(col + 1).ToString();
                }
                else if (col == -1)
                {
                    TextMeshProUGUI coords = Instantiate(CoordinateSlot, Vector3.zero, Quaternion.identity, transform.GetChild(0).transform).GetComponentInChildren<TextMeshProUGUI>();
                    coords.text = (row + 1).ToString();
                }
                else
                {
                    PuzzleSlots.Add(Instantiate(PuzzleSlot, Vector3.zero, Quaternion.identity, transform.GetChild(0).transform));
                    PuzzleSlots[^1].name = $"{PuzzleSlicer.IntToChar(col + 1)}-{row + 1}";
                }
            }
        }
    }

    public void CheckPuzzle()
    {
        int score = 0;
        for (int i = 0; i < PuzzleSlots.Count; i++)
        {
            if (PuzzleSlots[i].transform.childCount > 0)
            {
                if (PuzzleSlots[i].name.Equals(PuzzleSlots[i].transform.GetChild(0).name))
                {
                    PuzzleSlots[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                    score++;
                }
                else
                {
                    PuzzleSlots[i].transform.GetChild(0).GetComponent<Image>().color = Color.red;
                }
            }
            else
            {
                PuzzleSlots[i].GetComponent<Image>().color = Color.red;
            }
        }

        Coordinates.SetActive(false);
        ScoreText.text = score + "/" + PuzzleSlots.Count;

        GameObject[] puzzlePieces = GameObject.FindGameObjectsWithTag("PuzzlePiece");

        foreach (GameObject piece in puzzlePieces)
        {
            piece.GetComponent<PuzzlePiece>().enabled = false;
        }

        if (!_extraChance)
        {
            EndGame(ScoreText.text);
        }
        _extraChance = false;
    }

    public void ResetPuzzle()
    {
        for (int i = 0; i < PuzzleSlots.Count; i++)
        { 
            if (PuzzleSlots[i].transform.childCount > 0)
            {
                PuzzleSlots[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
            else
            {
                PuzzleSlots[i].GetComponent<Image>().color = Color.white;
            }
        }

        Coordinates.SetActive(true);
        ScoreText.text = "";

        GameObject[] puzzlePieces = GameObject.FindGameObjectsWithTag("PuzzlePiece");

        foreach (GameObject piece in puzzlePieces)
        {
            piece.GetComponent<PuzzlePiece>().enabled = true;
        }
    }

    public void EndGame(string score)
    {
        EndScreenLogic.EndGame("PuzzelGameMenu", "Coördinaten puzzel", $"{score}", PlayerPrefs.GetInt("puzzeldifficulty"), Camera.main.orthographicSize, 2);
        GameObject preview = GameObject.FindWithTag("Preview");
        preview.transform.SetParent(null);
        preview.transform.localScale = new(preview.transform.localScale.x * 0.75f, preview.transform.localScale.y * 0.75f, 1);
        DontDestroyOnLoad(preview);
        SceneManager.LoadScene("EndScreen");
    }
}
