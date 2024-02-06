using SFB;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] _puzzleMenus;
    [SerializeField] private GameObject _pzButton;
    [SerializeField] private GameObject _puzzles;
    [SerializeField] private Transform _pokemonPuzzles;
    [SerializeField] private GameObject _deleteButton;
    private int _currentPuzzleMenu = 0;

    private static Sprite _puzzleImage;
    private static int _difficulty;
    private static string _puzzleName;

    private GameObject[] PuzzleMenus { get => _puzzleMenus; set => _puzzleMenus = value; }
    private GameObject PzButton { get => _pzButton; set => _pzButton = value; }
    private GameObject Puzzles { get => _puzzles; set => _puzzles = value; }
    private Transform PokemonPuzzles { get => _pokemonPuzzles; set => _pokemonPuzzles = value; }
    private GameObject DeleteButton { get => _deleteButton; set => _deleteButton = value; }
    private int CurrentPuzzleMenu { get => _currentPuzzleMenu; set => _currentPuzzleMenu = value; }
    public static Sprite PuzzleImage { get => _puzzleImage; set => _puzzleImage = value; }
    public static int Difficulty { get => _difficulty; set => _difficulty = value; }
    public static string PuzzleName { get => _puzzleName; set => _puzzleName = value; }

    private void Awake()
    {
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Puzzels")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Puzzels"));
        }
        LoadPuzzles(Path.Combine(Application.streamingAssetsPath, "PuzzelGame/Puzzels"), "original");
        LoadPuzzles(Path.Combine(Application.persistentDataPath, "Puzzels"), "custom");
        PlayerPrefs.SetInt("puzzeldifficulty", 1);

        GameObject pokemonPuzzles = GameObject.FindWithTag("pokemonpuzzles");
        GameObject pokemonPuzzlesContent = Instantiate(pokemonPuzzles, PokemonPuzzles);
        pokemonPuzzlesContent.transform.SetAsFirstSibling();
        PokemonPuzzles.GetComponent<ScrollRect>().content = pokemonPuzzlesContent.GetComponent<RectTransform>();

    }

    public void Leave()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenMenu(int index)
    {
        PuzzleMenus[CurrentPuzzleMenu].SetActive(false);
        PuzzleMenus[index].SetActive(true);
        CurrentPuzzleMenu = index;
    }

    public void OpenDifficulty()
    {
        PuzzleMenus[2].SetActive(true);
        Image preview = GameObject.FindWithTag("Preview").GetComponent<Image>();
        preview.sprite = PuzzleImage;
        preview.preserveAspect = true;
        DeleteButton.SetActive(PuzzleName.Contains("custom-"));
    }

    public void CloseDifficulty()
    {
        PuzzleMenus[2].SetActive(false);
    }

    public void StartPuzzle()
    {
        SceneManager.LoadScene("PuzzelGame");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void AddPuzzle(Sprite puzzleImage, string name)
    {
        PuzzleButton puzzleButton = Instantiate(PzButton, new Vector2(0f, 0f), Quaternion.identity, Puzzles.transform).GetComponent<PuzzleButton>();
        puzzleButton.CreatePuzzle(puzzleImage);
        puzzleButton.name = name;
    }

    private void LoadPuzzles(string path, string name)
    {
        string[] pngFiles = Directory.GetFiles(path, "*.png");
        string[] jpgFiles = Directory.GetFiles(path, "*.jpg");
        string[] jpegFiles = Directory.GetFiles(path, "*.jpeg");
        string[] allImageFiles = pngFiles.Concat(jpgFiles).Concat(jpegFiles).ToArray();


        foreach (string imageFile in allImageFiles)
        {
            byte[] fileData = File.ReadAllBytes(imageFile);
            Texture2D texture = new(2, 2);
            texture.LoadImage(fileData);

            AddPuzzle(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)), name + "-" + Path.GetFileName(imageFile));
        }

        // Scroll to the top after adding all puzzles
        ScrollRect scrollRect = Puzzles.transform.parent.transform.parent.GetComponentInChildren<ScrollRect>();
        if (scrollRect != null)
        {
            // Set normalizedPosition to scroll to the top
            scrollRect.normalizedPosition = new Vector2(0, 1);

            // If you also have a scrollbar, you may need to update its value
            Scrollbar scrollbar = scrollRect.verticalScrollbar;
            if (scrollbar != null)
            {
                scrollbar.value = 1;
            }
        }
    }

    public void AddCustomPuzzle()
    {
        var extensions = new[] { new ExtensionFilter("Afbeeldingen", "png", "jpg", "jpeg") };
        string[] filePaths = StandaloneFileBrowser.OpenFilePanel("Kies een afbeelding", "", extensions, false);
        if (filePaths != null && filePaths.Length > 0 && !string.IsNullOrEmpty(filePaths[0]))
        {
            string imagePath = filePaths[0];
            byte[] fileData = File.ReadAllBytes(imagePath);
            Texture2D texture = new(2, 2);
            texture.LoadImage(fileData);

            AddPuzzle(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)), "custom-" + Path.GetFileName(imagePath));
            File.Copy(imagePath, Path.Combine(Application.persistentDataPath, "Puzzels", Path.GetFileName(imagePath)), true);
        }

        // Scroll to the top after adding all puzzles
        ScrollRect scrollRect = Puzzles.transform.parent.transform.parent.GetComponentInChildren<ScrollRect>();
        if (scrollRect != null)
        {
            // Set normalizedPosition to scroll to the top
            scrollRect.normalizedPosition = new Vector2(0, 0);

            // If you also have a scrollbar, you may need to update its value
            Scrollbar scrollbar = scrollRect.verticalScrollbar;
            if (scrollbar != null)
            {
                scrollbar.value = 0;
            }
        }
    }

    public void DeletePuzzle()
    {
        print(PuzzleName.Substring(7));
        File.Delete(Path.Combine(Application.persistentDataPath, "Puzzels", PuzzleName.Substring(7)));
        Destroy(GameObject.Find(PuzzleName));
    }
}
