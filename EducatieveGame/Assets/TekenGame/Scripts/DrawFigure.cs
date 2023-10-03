using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class DrawFigure : MonoBehaviour
{
    [SerializeField] private GameObject _startDot;
    [SerializeField] private string _figureName;
    [SerializeField] private TextMeshProUGUI _widthText;
    [SerializeField] private TextMeshProUGUI _heightText;
    [SerializeField] private TextMeshProUGUI _cellSizeText;
    [SerializeField] private GameObject _gridSettings;
    [SerializeField] private GameObject _drawMode;
    private int _cellSize = 1;
    private int _width = 2;
    private int _height = 2;
    private bool settingValues = true;
    private LineRenderer _lineRend;
    private GridGenerator _gridGen;
    private GameObject _start;
    private bool _startDotPlaced = false;
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

    private GameObject StartDot { get => _startDot; set => _startDot = value; }
    private string FigureName { get => _figureName; set => _figureName = value; }
    private LineRenderer LineRend { get => _lineRend; set => _lineRend = value; }
    private GridGenerator GridGen { get => _gridGen; set => _gridGen = value; }
    private Dictionary<(float, float), string> Directions { get => _directions; set => _directions = value; }
    private GameObject Start { get => _start; set => _start = value; }
    private bool StartDotPlaced { get => _startDotPlaced; set => _startDotPlaced = value; }
    private int CellSize { get => _cellSize; set => _cellSize = value; }
    private int Width { get => _width; set => _width = value; }
    private int Height { get => _height; set => _height = value; }
    private TextMeshProUGUI WidthText { get => _widthText; set => _widthText = value; }
    private TextMeshProUGUI HeightText { get => _heightText; set => _heightText = value; }
    private TextMeshProUGUI CellSizeText { get => _cellSizeText; set => _cellSizeText = value; }
    private bool SettingValues { get => settingValues; set => settingValues = value; }
    private GameObject GridSettings { get => _gridSettings; set => _gridSettings = value; }
    private GameObject DrawMode { get => _drawMode; set => _drawMode = value; }

    private void Awake()
    {
        LineRend = GetComponent<LineRenderer>();
        GridGen = GetComponent<GridGenerator>();
        FigureName = FigureName + ".txt";
        Start = Instantiate(StartDot, Vector3.zero, Quaternion.identity, transform);
    }

    private void Update()
    {
        if (SettingValues)
        {
            WidthText.text = Width.ToString();
            HeightText.text = Height.ToString();
            CellSizeText.text = CellSize.ToString();
        }

        else
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (MouseInGrid(mousePosition))
            {
                if (!StartDotPlaced)
                {
                    Vector3 closestPositionOnGrid = ClosestPositionOnGrid(mousePosition);
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
                    Vector3 closestPositionOnGrid = ClosestPosition(mousePosition, LineRend.GetPosition(LineRend.positionCount - 2));
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
        }
    }

    public void GenerateGrid()
    {
        settingValues = false;
        GridSettings.SetActive(false);
        DrawMode.SetActive(true);
        GridGen.GenerateGrid(Width, Height, CellSize);
    }

    public void ValueUp(int index)
    {
        if (index == 0)
        {
            if (Width < 16)
            {
                Width++;
            }
        }
        else if (index == 1)
        {
            if (Height < 10)
            {
                Height++;
            }
        }
        else if (index == 2)
        {
            if (CellSize < 2)
            {
                CellSize++;
            }
        }
    }

    public void ValueDown(int index)
    {
        if (index == 0)
        {
            if (Width > 2)
            {
                Width--;
            }
        }
        else if (index == 1)
        {
            if (Height > 2)
            {
                Height--;
            }
        }
        else if (index == 2)
        {
            if (CellSize > 1)
            {
                CellSize--;
            }
        }
    }

    private bool MouseInGrid(Vector3 position)
    {
        float minX = GridGen.GridPoints[0].x;
        float minY = GridGen.GridPoints[0].y;
        float maxX = GridGen.GridPoints[^1].x;
        float maxY = GridGen.GridPoints[^1].y;
        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }

    private Vector3 ClosestPositionOnGrid(Vector3 position)
    {
        Vector3 closestGridPoint = GridGen.GridPoints[0];
        float closestDistance = Vector3.Distance(position, closestGridPoint);

        for (int i = 1; i < GridGen.GridPoints.Length; i++)
        {
            float distance = Vector3.Distance(position, GridGen.GridPoints[i]);

            if (distance < closestDistance)
            {
                closestGridPoint = GridGen.GridPoints[i];
                closestDistance = distance;
            }
        }

        return closestGridPoint;
    }

    private Vector3 ClosestPosition(Vector3 position, Vector3 targetGridPoint)
    {
        Vector3 closestGridPoint = Vector3.zero;
        float closestDistanceToMouse = Vector3.Distance(position, closestGridPoint);

        for (int i = 0; i < GridGen.GridPoints.Length; i++)
        {
            Vector3 gridPoint = GridGen.GridPoints[i];
            float distanceToMouse = Vector3.Distance(position, gridPoint);

            if (Mathf.Abs(gridPoint.x - targetGridPoint.x) <= CellSize && Mathf.Abs(gridPoint.y - targetGridPoint.y) <= CellSize)
            {
                if (distanceToMouse < closestDistanceToMouse)
                {
                    closestGridPoint = gridPoint;
                    closestDistanceToMouse = distanceToMouse;
                }
            }
        }

        return closestGridPoint;
    }

    private void AddStartPos(Vector3 startPos)
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, "figures");
        string filePath = Path.Combine(directoryPath, FigureName);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("CellSize: " + CellSize);
            writer.WriteLine("Width: " + Width);
            writer.WriteLine("Height: " + Height);
            writer.WriteLine("StartPos: " + startPos.x + ", " + startPos.y + ", " + startPos.z);
            writer.WriteLine("LineSegments:");
        }
    }

    private void AddLineSegment(string direction)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "figures", FigureName);

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(direction);
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene("LoadFigure");
    }
}
