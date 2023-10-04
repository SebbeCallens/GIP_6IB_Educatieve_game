using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _cell;
    private Vector3[] _gridPoints;

    private GameObject Cell { get => _cell; set => _cell = value; }
    public Vector3[] GridPoints { get => _gridPoints; set => _gridPoints = value; }

    //genereert een grid met gegeven breedte, hoogte en grootte en bewaart de gridpunten in een array
    public void GenerateGrid(int width, int height, int cellSize)
    {
        int numberOfGridPoints = (width + 1) * (height + 1);
        GridPoints = new Vector3[numberOfGridPoints];

        float offsetX = 0.5f * cellSize; //hulpafstand voor het grid te centreren
        float offsetY = 0.5f * cellSize; //hulpafstand voor het grid te centreren

        if (cellSize % 2 == 1 && width % 2 == 1)
        {
            offsetX = 0;
        }
        if (cellSize % 2 == 1 && height % 2 == 1)
        {
            offsetY = 0;
        }

        int i = 0;
        for (int y = 0; y <= height; y++) //elke y coordinaat aflopen
        {
            for (int x = 0; x <= width; x++) //elke x coordinaat aflopen
            {
                Vector3 cellPosition = new(x * cellSize + transform.position.x - (width * cellSize) / 2 + offsetX, y * cellSize + transform.position.y - (height * cellSize) / 2 + offsetY, 0);
                
                if (y != height && x != width) //voor de laatste y rij en x rij geen cel object plaatsen
                {
                    GameObject cell = Instantiate(Cell, cellPosition, Quaternion.identity, transform);
                    cell.transform.localScale = new(cellSize, cellSize, 1);
                }

                GridPoints[i] = new(cellPosition.x - 0.5f * cellSize, cellPosition.y - 0.5f * cellSize, 0); //gridpunt instelllen in array
                i++;
            }
        }
    }
}
