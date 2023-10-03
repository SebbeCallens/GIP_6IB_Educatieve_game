using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _cell;
    private int _cellSize;
    private int _width;
    private int _height;
    private Vector3[] _gridPoints;

    private GameObject Cell { get => _cell; set => _cell = value; }
    public int Width { get => _width; set => _width = value; }
    public int Height { get => _height; set => _height = value; }
    public int CellSize { get => _cellSize; set => _cellSize = value; }
    public Vector3[] GridPoints { get => _gridPoints; set => _gridPoints = value; }

    public void GenerateGrid()
    {
        int numberOfGridPoints = (Width + 1) * (Height + 1);
        GridPoints = new Vector3[numberOfGridPoints];

        float offsetX = 0.5f * CellSize;
        float offsetY = 0.5f * CellSize;

        if (CellSize % 2 == 1 && Width % 2 == 1)
        {
            offsetX = 0;
        }
        if (CellSize % 2 == 1 && Height % 2 == 1)
        {
            offsetY = 0;
        }

        int i = 0;
        for (int y = 0; y <= Height; y++)
        {
            for (int x = 0; x <= Width; x++)
            {
                Vector3 cellPosition = new(x * CellSize + transform.position.x - (Width * CellSize) / 2 + offsetX, y * CellSize + transform.position.y - (Height * CellSize) / 2 + offsetY, 0);
                
                if (y != Height && x != Width)
                {
                    GameObject cell = Instantiate(Cell, cellPosition, Quaternion.identity, transform);
                    cell.transform.localScale = new(CellSize, CellSize, 1);
                }

                GridPoints[i] = new(cellPosition.x - 0.5f * CellSize, cellPosition.y - 0.5f * CellSize, 0);
                i++;
            }
        }
    }
}
