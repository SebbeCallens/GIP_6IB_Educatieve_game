using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PathGrid _grid; //het grid
    private Vector2 _currentPositon; //huidigie positie speler

    public Vector2 CurrentPositon { get => _currentPositon; set => _currentPositon = value; }

    private void Awake() //componenten instellen
    {
        _grid = GameObject.Find("Path").GetComponent<PathGrid>();
    }

    public void TryMove(PathTile tile) //speler laten bewegen naar tile als dit mogelijk is
    {
        Vector2 _tilePos = _grid.GetTilePosition(tile); //positie huidige tile

        //lijst met posities van tiles rond de huidige tile
        List<Vector2> adjacentPositions = new List<Vector2>
    {
        new Vector2(CurrentPositon.x + 1, CurrentPositon.y),
        new Vector2(CurrentPositon.x - 1, CurrentPositon.y),
        new Vector2(CurrentPositon.x, CurrentPositon.y + 1),
        new Vector2(CurrentPositon.x, CurrentPositon.y - 1)
    };

        //als het een tile is die naast de tile van de speler ligt en dit geen obstakel is, dan de speler verplaatsen
        if (adjacentPositions.Contains(_tilePos) && !tile.IsObstacle)
        {
            transform.parent = tile.transform;
            transform.position = transform.parent.position;
            CurrentPositon = _tilePos;
        } 
    }
}
