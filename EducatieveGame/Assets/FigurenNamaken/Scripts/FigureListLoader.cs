using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FigureListLoader : MonoBehaviour
{
    [SerializeField] private GameObject _figureButton;
    [SerializeField] private GameObject _noFigures;
    [SerializeField] private bool _original;
    [SerializeField] private Gradient _difficultyColor;

    private GameObject FigureButton { get => _figureButton; set => _figureButton = value; }
    private GameObject NoFigures { get => _noFigures; set => _noFigures = value; }
    private bool Original { get => _original; set => _original = value; }
    private Gradient DifficultyColor { get => _difficultyColor; set => _difficultyColor = value; }

    private void Awake() //datapad instellen en figuurknoppen aanmaken
    {
        string figureDirectory;

        if (Original)
        {
            figureDirectory = Path.Combine(Application.streamingAssetsPath, "TekenGame/Figures");
            AddButtons(figureDirectory);
        }
        else
        {
            figureDirectory = Path.Combine(Application.persistentDataPath, "Figuren");
            AddButtons(figureDirectory);
        }
    }

    private void AddButtons(string figureDirectory) //functie die een figuurknop aanmaakt voor elke figuur in het datapad, stelt de naam en de moeilijkheid in voor de knop
    {
        if (Directory.Exists(figureDirectory))
        {
            string[] figures = Directory.GetFiles(figureDirectory, "*.txt"); //lijst met de figuren
            
            if (figures.Length > 0)
            {
                List<(string figurePath, float difficulty)> figureList = new List<(string, float)>(); //lijst die voor elke figuur de moeilijkheidsgraad bijhoud

                foreach (string figure in figures) //berekent moeilijkheid voor elke figuur en voegt het toe aan een lijst
                {
                    float difficult;

                    using (StreamReader reader = new StreamReader(figure)) //leest het figuurbestand uit
                    {
                        string line;
                        float lines = -5; //ingesteld op -5 want de eerste 5 lijnen zijn info over de figuur
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

                        difficult = lines / cellSize / 78f; //moeilijkheid hangt af van uit hoeveel lijnen de figuur bestaat en de celgrootte

                        if (difficult > 1) //moeilijkheid 1 is het maximum
                        {
                            difficult = 1;
                        }
                    }

                    figureList.Add((figure, difficult));
                }

                figureList.Sort((a, b) => a.difficulty.CompareTo(b.difficulty)); //sorteer de figuren op moeilijkheidsgraad

                int i = 0;
                foreach ((string figurePath, float difficulty) in figureList) //leest de lijst met figuren en hun moeilijkheidsgraad en maakt voor elke figuur een knop aan
                {
                    GameObject figureBut = Instantiate(FigureButton, new Vector2(0f, 0f), Quaternion.identity, transform);
                    TextMeshProUGUI[] figureText = figureBut.GetComponentsInChildren<TextMeshProUGUI>();
                    figureText[0].text = Path.GetFileNameWithoutExtension(figurePath);
                    Image[] difficultyImages = figureBut.GetComponentsInChildren<Image>();
                    int difficultyInt = Mathf.RoundToInt(difficulty * 5);
                    if (difficultyInt == 0)
                    {
                        difficultyInt = 1;
                    }
                    difficultyImages[1].fillAmount = difficultyInt / 5f;
                    difficultyImages[0].color = DifficultyColor.Evaluate(difficultyInt / 5f);
                    i++;
                }
            }
            else
            {
                Instantiate(NoFigures, transform);
            }
        }
        else //als er geen figuren zijn wordt het geen figuren object geplaaatst
        {
            Instantiate(NoFigures, transform);
        }

        StartCoroutine(ResetScrollRect());
    }

    private IEnumerator ResetScrollRect()
    {
        yield return null;

        ScrollRect scrollRect = transform.parent.GetComponent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1;

            Scrollbar scrollbar = scrollRect.verticalScrollbar;
            if (scrollbar != null)
            {
                scrollbar.value = 1;
            }
        }
    }
}
