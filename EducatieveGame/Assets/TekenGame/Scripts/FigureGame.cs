using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class FigureGame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _end;
    [SerializeField] private GameObject _instructions;
    [SerializeField] private TextMeshProUGUI _count;
    [SerializeField] private RectTransform _arrow;
    [SerializeField] private Score _scoreObj;
    [Header("Other")]
    [SerializeField] private GameObject _startDot;
    [SerializeField] private LineRenderer _assistLineRend; //de linerenderer om te gebruiken in assist modus
    [SerializeField] private Material _lineCorrect;
    [SerializeField] private Material _lineWrong;
    [SerializeField] private bool _assistMode;
    [SerializeField] private string _figureName; //later laten kiezen door gebruiker in menu
    private LineRenderer _lineRend;
    private GridGenerator _gridGen;
    private GridFunctions _gridFuncs;
    private int _width;
    private int _height;
    private int _cellSize;
    private int i = 0;
    private int original;
    private List<Vector3> _linePoints;
    private List<(int, int)> _arrows;
    private Dictionary<(float, float), string> _directions = new Dictionary<(float, float), string>
    {
        { (-1, -1), "Left-Down" },
        { (0, -1), "Down" },
        { (1, -1), "Right-Down" },
        { (-1, 0), "Left" },
        { (1, 0), "Right" },
        { (-1, 1), "Left-Up" },
        { (0, 1), "Up" },
        { (1, 1), "Right-Up" }
    };

    private GameObject End { get => _end; set => _end = value; }
    private GameObject Instructions { get => _instructions; set => _instructions = value; }
    private TextMeshProUGUI Count { get => _count; set => _count = value; }
    private RectTransform Arrow { get => _arrow; set => _arrow = value; }
    public Score ScoreObj { get => _scoreObj; set => _scoreObj = value; }
    private GameObject StartDot { get => _startDot; set => _startDot = value; }
    private LineRenderer AssistLineRend { get => _assistLineRend; set => _assistLineRend = value; }
    private Material LineCorrect { get => _lineCorrect; set => _lineCorrect = value; }
    private Material LineWrong { get => _lineWrong; set => _lineWrong = value; }
    private bool AssistMode { get => _assistMode; set => _assistMode = value; }
    private string FigureName { get => _figureName; set => _figureName = value; }
    private LineRenderer LineRend { get => _lineRend; set => _lineRend = value; }
    private GridGenerator GridGen { get => _gridGen; set => _gridGen = value; }
    public GridFunctions GridFuncs { get => _gridFuncs; set => _gridFuncs = value; }
    private int Width { get => _width; set => _width = value; }
    private int Height { get => _height; set => _height = value; }
    private int CellSize { get => _cellSize; set => _cellSize = value; }
    private int I { get => i; set => i = value; }
    private int Original { get => original; set => original = value; }
    private List<Vector3> LinePoints { get => _linePoints; set => _linePoints = value; }
    private List<(int, int)> Arrows { get => _arrows; set => _arrows = value; }
    private Dictionary<(float, float), string> Directions { get => _directions; set => _directions = value; }

    private void Awake()
    {
        LineRend = gameObject.GetComponent<LineRenderer>();
        GridGen = gameObject.GetComponent<GridGenerator>();
        GridFuncs = GetComponent<GridFunctions>();
        Original = PlayerPrefs.GetInt("original");
        FigureName = PlayerPrefs.GetString("figure");
        FigureName = FigureName + ".txt";
        LinePoints = new List<Vector3>();
        Arrows = new List<(int, int)>();
        ReadFigure();

        if (PlayerPrefs.GetInt("assist") == 1)
        {
            AssistMode = true;
        }
        else
        {
            AssistMode = false;
        }
    }

    private void Update()
    {
        if (I == 0) //startpunt plaatsen zodat de gebruiker weet waar de figuur begint
        {
            Arrows = TransformList(Arrows);
            LineRend.positionCount++;
            LineRend.SetPosition(LineRend.positionCount - 1, LinePoints[I]);
            if (AssistMode)
            {
                AssistLineRend.positionCount++;
                AssistLineRend.SetPosition(AssistLineRend.positionCount - 1, LinePoints[I]);
                AssistLineRend.positionCount++;
                AssistLineRend.SetPosition(AssistLineRend.positionCount - 1, LinePoints[I]);
            }
            else
            {
                AssistLineRend.positionCount++;
                AssistLineRend.SetPosition(AssistLineRend.positionCount - 1, LinePoints[I]);
                LineRend.positionCount++;
                LineRend.SetPosition(LineRend.positionCount - 1, LinePoints[I]);
            }
            I++;
        }

        else if (I != LinePoints.Count) //de gebruiker de figuur laten tekenen
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int count;
            int rotation;
            (count, rotation) = Arrows[I - 1];
            Count.text = count.ToString();
            Arrow.localEulerAngles = new Vector3(Arrow.localEulerAngles.x, Arrow.localEulerAngles.y, rotation);

            if (GridFuncs.MouseInGrid(mousePosition))
            {
                Vector3 closestPositionOnGrid;

                if (AssistMode)
                {
                    closestPositionOnGrid = GridFuncs.ClosestPosition(mousePosition, LineRend.GetPosition(LineRend.positionCount - 1), CellSize);
                    AssistLineRend.SetPosition(AssistLineRend.positionCount - 1, closestPositionOnGrid);
                }
                else
                {
                    closestPositionOnGrid = GridFuncs.ClosestPosition(mousePosition, LineRend.GetPosition(LineRend.positionCount - 2), CellSize);
                    LineRend.SetPosition(LineRend.positionCount - 1, closestPositionOnGrid);
                }

                if (closestPositionOnGrid == LinePoints[I])
                {
                    AssistLineRend.sharedMaterial = LineCorrect;

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (AssistMode)
                        {
                            AssistLineRend.SetPosition(AssistLineRend.positionCount - 2, LinePoints[I]);
                            AssistLineRend.SetPosition(AssistLineRend.positionCount - 1, LinePoints[I]);
                        }
                        else
                        {
                            ScoreObj.AddScore(1);
                        }

                        LineRend.positionCount++;
                        LineRend.SetPosition(LineRend.positionCount - 1, LinePoints[I]);
                        I++;
                    }
                }

                else
                {
                    AssistLineRend.sharedMaterial = LineWrong;
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        ScoreObj.AddScore(-1);
                    }
                }
            }
            else //ervoor zorgen dat als de muis niet in het grid is dat de laatste lijn niet blijft staan
            {
                if (AssistMode)
                {
                    AssistLineRend.SetPosition(AssistLineRend.positionCount - 1, AssistLineRend.GetPosition(AssistLineRend.positionCount - 2));
                }
                else
                {
                    LineRend.SetPosition(LineRend.positionCount - 1, LineRend.GetPosition(LineRend.positionCount - 2));
                }
            }
        }
        else //figuur is af, UI updaten
        {
            if (AssistLineRend.positionCount > 0)
            {
                AssistLineRend.positionCount = 0;
                Instructions.SetActive(false);
                End.SetActive(true);
            }
        }
    }

    private List<(int, int)> TransformList(List<(int, int)> inputList) //lijst met instructies aanpassen zodat als het meerdere keren na elkaar dezelfde kant op is dat dit klopt in de instructie
    {
        List<(int, int)> transformedList = new List<(int, int)>();

        int count = 1;

        for (int i = 1; i < inputList.Count; i++)
        {
            if (inputList[i].Item2 == inputList[i - 1].Item2)
            {
                count++;
            }
            else
            {
                if (count > 1)
                {
                    for (int j = 0; j < count; j++)
                    {
                        transformedList.Add((count, inputList[i - 1].Item2));
                    }
                }
                else
                {
                    transformedList.Add((1, inputList[i - 1].Item2));
                }
                count = 1;
            }
        }

        if (count > 1)
        {
            for (int j = 0; j < count; j++)
            {
                transformedList.Add((count, inputList[inputList.Count - 1].Item2));
            }
        }
        else
        {
            transformedList.Add((1, inputList[inputList.Count - 1].Item2));
        }

        return transformedList;
    }

    private void ReadFigure() //leest de lijnen van het figuurbestand
    {
        string filePath;
        if (Original == 0)
        {
            filePath = Path.Combine(Application.persistentDataPath, "figures", FigureName);
        }
        else
        {
            filePath = Path.Combine(Application.streamingAssetsPath, "TekenGame/Figures", FigureName);
        }

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                bool readingLineSegments = false;
                Vector3 startPos = Vector3.zero;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("CellSize: "))
                    {
                        string cellSizeString = line.Substring("CellSize: ".Length);
                        CellSize = int.Parse(cellSizeString.Trim());
                    }
                    else if (line.StartsWith("Width: "))
                    {
                        string widthString = line.Substring("Width: ".Length);
                        Width = int.Parse(widthString.Trim());
                    }
                    else if (line.StartsWith("Height: "))
                    {
                        string heightString = line.Substring("Height: ".Length);
                        Height = int.Parse(heightString.Trim());
                    }
                    else if (line.StartsWith("StartPos: "))
                    {
                        GridGen.GenerateGrid(Width, Height, CellSize);

                        string startPosString = line.Substring("StartPos: ".Length);
                        string[] startPosParts = startPosString.Split(',');
                        if (startPosParts.Length == 3)
                        {
                            float startX = float.Parse(startPosParts[0].Trim());
                            float startY = float.Parse(startPosParts[1].Trim());
                            float startZ = float.Parse(startPosParts[2].Trim());
                            startPos = new(startX, startY, startZ);
                            Instantiate(StartDot, startPos, Quaternion.identity, transform);
                            LinePoints.Add(startPos);
                        }
                    }
                    else if (line.StartsWith("LineSegments:"))
                    {
                        readingLineSegments = true;
                    }
                    else if (readingLineSegments)
                    {
                        if (Directions.ContainsValue(line.Trim()))
                        {
                            Vector3 previousPoint = LinePoints.Count > 0 ? LinePoints[LinePoints.Count - 1] : startPos;
                            Vector3 newPoint = ComputeNextPoint(previousPoint, line.Trim());

                            if (line.Trim() == "Right")
                            {
                                Arrows.Add((1, 0));
                            }
                            else if (line.Trim() == "Right-Up")
                            {
                                Arrows.Add((1, 45));
                            }
                            else if (line.Trim() == "Up")
                            {
                                Arrows.Add((1, 90));
                            }
                            else if (line.Trim() == "Left-Up")
                            {
                                Arrows.Add((1, 135));
                            }
                            else if (line.Trim() == "Left")
                            {
                                Arrows.Add((1, 180));
                            }
                            else if (line.Trim() == "Left-Down")
                            {
                                Arrows.Add((1, 225));
                            }
                            else if (line.Trim() == "Down")
                            {
                                Arrows.Add((1, 270));
                            }
                            else if (line.Trim() == "Right-Down")
                            {
                                Arrows.Add((1, 315));
                            }
                            LinePoints.Add(newPoint);
                        }
                    }
                }
            }
        }
    }

    private Vector3 ComputeNextPoint(Vector3 startPoint, string direction)
    {
        float dx = 0, dy = 0;
        foreach (var entry in Directions)
        {
            if (entry.Value == direction)
            {
                dx = entry.Key.Item1;
                dy = entry.Key.Item2;
                break;
            }
        }

        return new Vector3(startPoint.x + dx * CellSize, startPoint.y + dy * CellSize, startPoint.z);
    }
}
