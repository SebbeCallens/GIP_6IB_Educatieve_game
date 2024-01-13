using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 _currentPosition; //huidigie positie speler
    private int _steps = 0;

    public Vector2 CurrentPosition { get => _currentPosition; set => _currentPosition = value; }
    public int Steps { get => _steps; private set => _steps = value; }

    public bool TryMove(PathTile tile, Vector2 tilePosition) //speler laten bewegen naar tile als dit mogelijk is
    {
        //lijst met posities van tiles rond de huidige tile
        List<Vector2> adjacentPositions = new List<Vector2> 
        {
        new Vector2(CurrentPosition.x + 1, CurrentPosition.y),
        new Vector2(CurrentPosition.x - 1, CurrentPosition.y),
        new Vector2(CurrentPosition.x, CurrentPosition.y + 1),
        new Vector2(CurrentPosition.x, CurrentPosition.y - 1)
        };

        //als het een tile is die naast de tile van de speler ligt en dit geen obstakel is, dan de speler verplaatsen
        if (adjacentPositions.Contains(tilePosition) && !tile.IsObstacle)
        {
            transform.parent = tile.transform;
            transform.position = transform.parent.position;
            CurrentPosition = tilePosition;
            Steps++;
            return true;
        }
        return false;
    }
}
