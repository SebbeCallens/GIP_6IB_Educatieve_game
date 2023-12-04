using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//dit script is voor extra functies voor het grid en pad
public class PathFunctions : MonoBehaviour
{
    [SerializeField] private PathGrid _grid; //het grid
    [SerializeField] private PathManager _path; //het pad

    public PathTile GetRandomTile(List<PathTile> possibleTiles, Vector2 spawnPosition) //een random tile die afhangt van speler spawnpositie
    {
        Vector2 gridCenter = new Vector2(_grid.Width / 2, _grid.Height / 2);
        float[] weights = new float[possibleTiles.Count];

        for (int j = 0; j < possibleTiles.Count; j++)
        {
            Vector2 possibleTilePos = _grid.GetTilePosition(possibleTiles[j]);
            float distanceX = Mathf.Abs(spawnPosition.x - gridCenter.x);
            float distanceY = Mathf.Abs(spawnPosition.y - gridCenter.y);

            // Calculate the weight for the current tile
            weights[j] = (spawnPosition.x - possibleTilePos.x) * distanceX +
                         (spawnPosition.y - possibleTilePos.y) * distanceY +
                         (spawnPosition.x - possibleTilePos.x) * 0.5f +
                         (spawnPosition.y - possibleTilePos.y) * 0.5f;
        }

        // Normalize weights to ensure they add up to 1
        float totalWeight = weights.Sum();
        for (int j = 0; j < weights.Length; j++)
        {
            weights[j] /= totalWeight;
        }

        // Choose a random tile based on weights
        float randomValue = Random.value;
        float cumulativeWeight = 0;

        for (int j = 0; j < weights.Length; j++)
        {
            cumulativeWeight += weights[j];
            if (randomValue <= cumulativeWeight)
            {
                return possibleTiles[j];
            }
        }

        // If no tile is selected, return the last one
        return possibleTiles.Last();
    }


    public List<PathTile> GetPossibleTiles(PathTile currentTile) //mogelijke volgende tiles voor het pad
    {
        Vector2 currentTilePos = _grid.GetTilePosition(currentTile); //positie huidige tile

        //lijst met posities van tiles rond de huidige tile
        List<Vector2> adjacentPositions = new List<Vector2>
    {
        new Vector2(currentTilePos.x + 1, currentTilePos.y),
        new Vector2(currentTilePos.x - 1, currentTilePos.y),
        new Vector2(currentTilePos.x, currentTilePos.y + 1),
        new Vector2(currentTilePos.x, currentTilePos.y - 1)
    };

        //lijst maken met de mogelijke tiles
        List<PathTile> possibleTiles = new List<PathTile>();

        foreach (Vector2 position in adjacentPositions)
        {
            PathTile tileAtPosition = _grid.GetTileAtPosition(position);

            //tile toevoegen wanneer deze bestaat en deze nog niet gebruikt is in het pad
            if (tileAtPosition != null && !_path.VisitedTiles.Contains(tileAtPosition))
            {
                possibleTiles.Add(tileAtPosition);
            }
        }

        return possibleTiles;
    }
}
