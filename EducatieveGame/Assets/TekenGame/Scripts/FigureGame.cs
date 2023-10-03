using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class FigureGame : MonoBehaviour
{
    [SerializeField] private GameObject _startDot;
    [SerializeField] private RectTransform _arrow;
    [SerializeField] private string _figureName;
    [SerializeField] private LineRenderer _extraLineRend;
    [SerializeField] private Material _lineCorrect;
    [SerializeField] private Material _lineWrong;
    [SerializeField] private TextMeshProUGUI _count;
    private LineRenderer _lineRend;
    private GridGenerator _gridGen;
    private List<Vector3> _linePoints;
    private List<(int, int)> _arrows;
    private int _cellSize;
    private int _width;
    private int _height;
    private int i = 0;
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
    private int CellSize { get => _cellSize; set => _cellSize = value; }
    private int Width { get => _width; set => _width = value; }
    private int Height { get => _height; set => _height = value; }
    private Dictionary<(float, float), string> Directions { get => _directions; set => _directions = value; }
    private RectTransform Arrow { get => _arrow; set => _arrow = value; }
    private List<Vector3> LinePoints { get => _linePoints; set => _linePoints = value; }
    private LineRenderer ExtraLineRend { get => _extraLineRend; set => _extraLineRend = value; }
    private Material LineCorrect { get => _lineCorrect; set => _lineCorrect = value; }
    private Material LineWrong { get => _lineWrong; set => _lineWrong = value; }
    private List<(int, int)> Arrows { get => _arrows; set => _arrows = value; }
    private TextMeshProUGUI Count { get => _count; set => _count = value; }

    private void Awake()
    {
        LineRend = gameObject.GetComponent<LineRenderer>();
        GridGen = gameObject.GetComponent<GridGenerator>();
        FigureName = FigureName + ".txt";
        LinePoints = new List<Vector3>();
        Arrows = new List<(int, int)>();
        ReadFigure();
    }

    private void Update()
    {
        if (i == 0)
        {
            Arrows = TransformList(Arrows);
            LineRend.positionCount++;
            LineRend.SetPosition(LineRend.positionCount - 1, LinePoints[i]);
            ExtraLineRend.positionCount++;
            ExtraLineRend.SetPosition(ExtraLineRend.positionCount - 1, LinePoints[i]);
            ExtraLineRend.positionCount++;
            ExtraLineRend.SetPosition(ExtraLineRend.positionCount - 1, LinePoints[i]);
            i++;
        }

        else if (i != LinePoints.Count)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int count;
            int rotation;
            (count, rotation) = Arrows[i - 1];
            Count.text = count.ToString();
            Arrow.localEulerAngles = new Vector3(Arrow.localEulerAngles.x, Arrow.localEulerAngles.y, rotation);

            if (MouseInGrid(mousePosition))
            {
                Vector3 closestPositionOnGrid = ClosestPosition(mousePosition, LineRend.GetPosition(LineRend.positionCount - 1));
                ExtraLineRend.SetPosition(ExtraLineRend.positionCount - 1, closestPositionOnGrid);

                if (closestPositionOnGrid == LinePoints[i])
                {
                    ExtraLineRend.sharedMaterial = LineCorrect;

                    if (Input.GetMouseButtonDown(0))
                    {
                        LineRend.positionCount++;
                        LineRend.SetPosition(LineRend.positionCount - 1, LinePoints[i]);
                        ExtraLineRend.positionCount++;
                        ExtraLineRend.SetPosition(ExtraLineRend.positionCount - 1, LinePoints[i]);
                        i++;
                    }
                }

                else
                {
                    ExtraLineRend.sharedMaterial = LineWrong;
                }
            }
        }
    }

    private List<(int, int)> TransformList(List<(int, int)> inputList)
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

    private bool MouseInGrid(Vector3 position)
    {
        float minX = GridGen.GridPoints[0].x;
        float minY = GridGen.GridPoints[0].y;
        float maxX = GridGen.GridPoints[^1].x;
        float maxY = GridGen.GridPoints[^1].y;
        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
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

    private void ReadFigure()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "figures", FigureName);

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
                        string widthString = line.Substring("CellSize: ".Length);
                        CellSize = int.Parse(widthString.Trim());
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
                        GridGen.CellSize = CellSize;
                        GridGen.Width = Width;
                        GridGen.Height = Height;
                        GridGen.GenerateGrid();

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
