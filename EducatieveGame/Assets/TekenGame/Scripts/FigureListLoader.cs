using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
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
            AddButtons(figureDirectory);
        }
        else
        {
            figureDirectory = Path.Combine(Application.persistentDataPath, "figures");
            AddButtons(figureDirectory);
        }
    }

    private void AddButtons(string figureDirectory)
    {
        if (Directory.Exists(figureDirectory))
        {
            string[] figures = Directory.GetFiles(figureDirectory, "*.txt");
            List<(string figurePath, float difficulty)> figureList = new List<(string, float)>();

            foreach (string figure in figures)
            {
                float difficult;

                using (StreamReader reader = new StreamReader(figure))
                {
                    string line;
                    float lines = 0;
                    float cellSize = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("CellSize: "))
                        {
                            string cellSizeString = line.Substring("CellSize: ".Length);
                            cellSize = int.Parse(cellSizeString.Trim());
                        }
                        lines++;
                    }

                    difficult = lines / cellSize / 100;

                    if (difficult > 1)
                    {
                        difficult = 1;
                    }
                }

                figureList.Add((figure, difficult));
            }

            figureList.Sort((a, b) => a.difficulty.CompareTo(b.difficulty));

            int i = 0;
            foreach ((string figurePath, float difficulty) in figureList)
            {
                GameObject figureBut = Instantiate(FigureButton, transform);
                TextMeshProUGUI[] figureText = figureBut.GetComponentsInChildren<TextMeshProUGUI>();
                figureText[0].text = Path.GetFileNameWithoutExtension(figurePath);
                Image[] difficultyImages = figureBut.GetComponentsInChildren<Image>();
                difficultyImages[1].fillAmount = difficulty;
                i++;
            }
        }
        else
        {
            Instantiate(NoFigures, transform);
        }
    }
}
