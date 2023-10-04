using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class DrawFigure : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField _figureNameText;
    [SerializeField] private TextMeshProUGUI _widthText;
    [SerializeField] private TextMeshProUGUI _heightText;
    [SerializeField] private TextMeshProUGUI _cellSizeText;
    [SerializeField] private GameObject _gridSettings;
    [SerializeField] private GameObject _drawMode;
    [SerializeField] private GameObject _warning;
    [SerializeField] private GameObject _noLinesWarning;
    [Header("Settings")]
    [SerializeField] private int _minWidth;
    [SerializeField] private int _minHeight;
    [SerializeField] private int _maxWidth;
    [SerializeField] private int _maxHeight;
    [Header("Other")]
    [SerializeField] private GameObject _startDot;
    [SerializeField] private GameObject _menuLogObject;
    private string _figureName;
    private int _width;
    private int _height;
    private int _cellSize = 1;
    private bool _settingGridValues = true;
    private bool _startDotPlaced = false;
    private bool _noLines = true;
    private GameObject _start;
    private LineRenderer _lineRend;
    private GridGenerator _gridGen;
    private GridFunctions _gridFuncs;
    private MenuLogic _menuLog;
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

    private TMP_InputField FigureNameText { get => _figureNameText; set => _figureNameText = value; }
    private TextMeshProUGUI WidthText { get => _widthText; set => _widthText = value; }
    private TextMeshProUGUI HeightText { get => _heightText; set => _heightText = value; }
    private TextMeshProUGUI CellSizeText { get => _cellSizeText; set => _cellSizeText = value; }
    private GameObject GridSettings { get => _gridSettings; set => _gridSettings = value; }
    private GameObject DrawMode { get => _drawMode; set => _drawMode = value; }
    private GameObject Warning { get => _warning; set => _warning = value; }
    private GameObject NoLinesWarning { get => _noLinesWarning; set => _noLinesWarning = value; }
    private int MinWidth { get => _minWidth; set => _minWidth = value; }
    private int MinHeight { get => _minHeight; set => _minHeight = value; }
    private int MaxWidth { get => _maxWidth; set => _maxWidth = value; }
    private int MaxHeight { get => _maxHeight; set => _maxHeight = value; }
    private GameObject StartDot { get => _startDot; set => _startDot = value; }
    private GameObject MenuLogObject { get => _menuLogObject; set => _menuLogObject = value; }
    private string FigureName { get => _figureName; set => _figureName = value; }
    private int Width { get => _width; set => _width = value; }
    private int Height { get => _height; set => _height = value; }
    private int CellSize { get => _cellSize; set => _cellSize = value; }
    private bool SettingGridValues { get => _settingGridValues; set => _settingGridValues = value; }
    private bool StartDotPlaced { get => _startDotPlaced; set => _startDotPlaced = value; }
    private bool NoLines { get => _noLines; set => _noLines = value; }
    private GameObject Start { get => _start; set => _start = value; }
    private LineRenderer LineRend { get => _lineRend; set => _lineRend = value; }
    private GridGenerator GridGen { get => _gridGen; set => _gridGen = value; }
    private GridFunctions GridFuncs { get => _gridFuncs; set => _gridFuncs = value; }
    private MenuLogic MenuLog { get => _menuLog; set => _menuLog = value; }
    private Dictionary<(float, float), string> Directions { get => _directions; set => _directions = value; }

    private void Awake()
    {
        LineRend = GetComponent<LineRenderer>();
        GridGen = GetComponent<GridGenerator>();
        GridFuncs = GetComponent<GridFunctions>();
        MenuLog = MenuLogObject.GetComponent<MenuLogic>();
        Start = Instantiate(StartDot, Vector3.zero, Quaternion.identity, transform);
        Width = MinWidth;
        Height = MinHeight;
        WidthText.text = Width.ToString();
        HeightText.text = Height.ToString();
        CellSizeText.text = CellSize.ToString();
    }

    private void Update()
    {
        if (!SettingGridValues)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GridFuncs.MouseInGrid(mousePosition))
            {
                if (!StartDotPlaced)
                {
                    Vector3 closestPositionOnGrid = GridFuncs.ClosestPositionOnGrid(mousePosition);
                    Start.transform.position = closestPositionOnGrid;
                    if (Input.GetMouseButtonDown(0))
                    {
                        StartDotPlaced = true;
                        AddStartPos(closestPositionOnGrid);
                        LineRend.positionCount++;
                        LineRend.SetPosition(LineRend.positionCount - 1, closestPositionOnGrid);
                        LineRend.positionCount++;
                        LineRend.SetPosition(LineRend.positionCount - 1, closestPositionOnGrid);
                    }
                }

                else
                {
                    Vector3 closestPositionOnGrid = GridFuncs.ClosestPosition(mousePosition, LineRend.GetPosition(LineRend.positionCount - 2), CellSize);
                    LineRend.SetPosition(LineRend.positionCount - 1, closestPositionOnGrid);

                    if (Directions.ContainsKey(((closestPositionOnGrid.x - LineRend.GetPosition(LineRend.positionCount - 2).x) / CellSize, (closestPositionOnGrid.y - LineRend.GetPosition(LineRend.positionCount - 2).y) / CellSize)) && Input.GetMouseButtonDown(0))
                    {
                        string direction = Directions[((closestPositionOnGrid.x - LineRend.GetPosition(LineRend.positionCount - 2).x) / CellSize, (closestPositionOnGrid.y - LineRend.GetPosition(LineRend.positionCount - 2).y) / CellSize)];
                        AddLineSegment(direction);
                        LineRend.positionCount++;
                        LineRend.SetPosition(LineRend.positionCount - 1, closestPositionOnGrid);
                    }
                }
            }
            else if (StartDotPlaced) //ervoor zorgen dat als de muis niet in het grid is dat de laatste lijn niet blijft staan
            {
                LineRend.SetPosition(LineRend.positionCount - 1, LineRend.GetPosition(LineRend.positionCount - 2));
            }
            else //ervoor zorgen als de muis niet in het grid is dat het startpunt dat nog niet gezet is verdwijnt
            {
                Start.transform.position = Vector3.zero;
            }
        }
    }

    public void GenerateGrid() //genereer het grid, verberg de gridinstellingen en maak de instellingen voor tekenmodus zichtbaar
    {
        if (!string.IsNullOrWhiteSpace(FigureNameText.text))
        {
            SettingGridValues = false;
            GridSettings.SetActive(false);
            DrawMode.SetActive(true);
            GridGen.GenerateGrid(Width, Height, CellSize);
            FigureName = FigureNameText.text + ".txt";
        }
        else
        {
            Warning.SetActive(true);
        }
    }

    public void ValueUp(int index)
    {
        if (index == 0)
        {
            if (Width < MaxWidth / CellSize)
            {
                Width++;
                WidthText.text = Width.ToString();
            }
        }
        else if (index == 1)
        {
            if (Height < MaxHeight / CellSize)
            {
                Height++;
                HeightText.text = Height.ToString();
            }
        }
        else if (index == 2)
        {
            if (CellSize < 2)
            {
                CellSize++;
                if (Width > MaxWidth / CellSize)
                {
                    Width = MaxWidth / CellSize;
                    WidthText.text = Width.ToString();
                }
                if (Height > MaxHeight / CellSize)
                {
                    Height = MaxHeight / CellSize;
                    HeightText.text = Height.ToString();
                }
                CellSizeText.text = CellSize.ToString();
            }
        }
    }

    public void ValueDown(int index)
    {
        if (index == 0)
        {
            if (Width > MinWidth / CellSize)
            {
                Width--;
                WidthText.text = Width.ToString();
            }
        }
        else if (index == 1)
        {
            if (Height > MinHeight / CellSize)
            {
                Height--;
                HeightText.text = Height.ToString();
            }
        }
        else if (index == 2)
        {
            if (CellSize > 1)
            {
                CellSize--;
                if (Width < MinWidth * CellSize)
                {
                    Width = MinWidth * CellSize;
                    WidthText.text = Width.ToString();
                }
                if (Height < MinHeight * CellSize)
                {
                    Height = MinHeight * CellSize;
                    HeightText.text = Height.ToString();
                }
                CellSizeText.text = CellSize.ToString();
            }
        }
    }

    private void AddStartPos(Vector3 startPos) //voegt de instellingen van het grid van de huidige figuur toe en het startpunt van de figuur
    {
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "figures"))) //maakt de map voor figuren op te slaan aan als deze nog niet bestaat
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "figures"));
        }

        string filePath = Path.Combine(Application.persistentDataPath, "figures", FigureName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("CellSize: " + CellSize);
            writer.WriteLine("Width: " + Width);
            writer.WriteLine("Height: " + Height);
            writer.WriteLine("StartPos: " + startPos.x + ", " + startPos.y + ", " + startPos.z);
            writer.WriteLine("LineSegments:");
        }
    }

    private void AddLineSegment(string direction) //voegt de delen van de figuur toe aan het figuurbestand
    {
        NoLines = false;
        string filePath = Path.Combine(Application.persistentDataPath, "figures", FigureName);

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(direction);
        }
    }

    public void Done()
    {
        if (NoLines)
        {
            NoLinesWarning.SetActive(true);
        }
        else
        {
            MenuLog.LoadScene("SelectMode");
        }
    }
}
