using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFigure : MonoBehaviour
{
    [SerializeField] private GameObject _startDot;
    [SerializeField] private string _figureName;
    private LineRenderer _lineRend;
    private GridGenerator _gridGen;
    private int _width;
    private int _height;
    private int _cellSize;
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
    private int Width { get => _width; set => _width = value; }
    private int Height { get => _height; set => _height = value; }
    private int CellSize { get => _cellSize; set => _cellSize = value; }
    private Dictionary<(float, float), string> Directions { get => _directions; set => _directions = value; }

    private void Awake()
    {
        LineRend = gameObject.GetComponent<LineRenderer>();
        GridGen = gameObject.GetComponent<GridGenerator>();
        FigureName = FigureName + ".txt";
        ReadFigure();
    }

    private void ReadFigure()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "figures", FigureName);
        List<Vector3> linePoints = new List<Vector3>();

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
                            linePoints.Add(startPos);
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
                            Vector3 previousPoint = linePoints.Count > 0 ? linePoints[linePoints.Count - 1] : startPos;
                            Vector3 newPoint = ComputeNextPoint(previousPoint, line.Trim());
                            linePoints.Add(newPoint);
                        }
                    }
                }
            }
            LineRend.positionCount = linePoints.Count;
            LineRend.SetPositions(linePoints.ToArray());
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

    public void NextScene()
    {
        SceneManager.LoadScene("game");
    }
}
