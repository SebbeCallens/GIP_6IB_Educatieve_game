using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FigureListLoader : MonoBehaviour
{
    [SerializeField] private GameObject _figureButton;
    [SerializeField] private GameObject _figureButtonOriginal;
    [SerializeField] private GameObject _noFigures;
    [SerializeField] private bool _original;

    private GameObject FigureButton { get => _figureButton; set => _figureButton = value; }
    private GameObject FigureButtonOriginal { get => _figureButtonOriginal; set => _figureButtonOriginal = value; }
    private GameObject NoFigures { get => _noFigures; set => _noFigures = value; }
    private bool Original { get => _original; set => _original = value; }

    private void Awake()
    {
        string figureDirectory;

        if (Original)
        {
            figureDirectory = Path.Combine(Application.streamingAssetsPath, "TekenGame/Figures");

            if (Directory.Exists(figureDirectory))
            {
                string[] figures = Directory.GetFiles(figureDirectory, "*.txt");

                foreach (string figure in figures)
                {
                    GameObject figureBut = Instantiate(FigureButtonOriginal, transform);
                    TextMeshProUGUI textMeshPro = figureBut.GetComponentInChildren<TextMeshProUGUI>();
                    textMeshPro.text = Path.GetFileNameWithoutExtension(figure);
                }
            }
            else
            {
                Instantiate(NoFigures, transform);
            }
        }
        else
        {
            figureDirectory = Path.Combine(Application.persistentDataPath, "figures");

            if (Directory.Exists(figureDirectory))
            {
                string[] figures = Directory.GetFiles(figureDirectory, "*.txt");

                foreach (string figure in figures)
                {
                    GameObject figureBut = Instantiate(FigureButton, transform);
                    TextMeshProUGUI textMeshPro = figureBut.GetComponentInChildren<TextMeshProUGUI>();
                    textMeshPro.text = Path.GetFileNameWithoutExtension(figure);
                }
            }
            else
            {
                Instantiate(NoFigures, transform);
            }
        }
    }
}
