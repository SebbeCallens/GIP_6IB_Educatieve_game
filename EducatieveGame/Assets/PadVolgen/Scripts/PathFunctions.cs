using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//dit script is voor extra functies voor het grid en pad
public class PathFunctions : MonoBehaviour
{
    [SerializeField] private PathManager _pad; //het pad

    private PathManager Pad { get => _pad; set => _pad = value; }

    public PathTile GetRandomTile(List<PathTile> possibleTiles, Vector2 spawnPosition)
    {
        Vector2 gridCenter = new Vector2(Pad.Grid.Width / 2, Pad.Grid.Height / 2);
        float[] weights = new float[possibleTiles.Count];

        for (int j = 0; j < possibleTiles.Count; j++)
        {
            Vector2 possibleTilePos = Pad.Grid.GetTilePosition(possibleTiles[j]);
            float distanceX = Mathf.Abs(spawnPosition.x - gridCenter.x);
            float distanceY = Mathf.Abs(spawnPosition.y - gridCenter.y);

            // Calculate the weight for the current tile
            weights[j] = (spawnPosition.x - possibleTilePos.x) * distanceX +
                         (spawnPosition.y - possibleTilePos.y) * distanceY +
                         (spawnPosition.x - possibleTilePos.x) * 0.5f +
                         (spawnPosition.y - possibleTilePos.y) * 0.5f;

            // Introduce randomness to the weights
            weights[j] *= Random.Range(0.5f, 1.5f);
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
        Vector2 currentTilePos = Pad.Grid.GetTilePosition(currentTile); //positie huidige tile

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
            PathTile tileAtPosition = Pad.Grid.GetTileAtPosition(position);

            //tile toevoegen wanneer deze bestaat en deze nog niet gebruikt is in het pad, en deze tile geen aanliggende tile die bij het pad hoort heeft
            if (tileAtPosition != null && !Pad.RandomPath.Contains(tileAtPosition) && TileNeighborCount(tileAtPosition) < 2)
            {
                possibleTiles.Add(tileAtPosition);
            }
        }

        return possibleTiles;
    }

    public int TileNeighborCount(PathTile tile) //aantal buren van een tile die bij het random pad horen
    {
        int neighbors = 0;
        Vector2 tilePos = Pad.Grid.GetTilePosition(tile); //positie huidige tile

        List<Vector2> adjacentPositions = new List<Vector2>
    {
        new Vector2(tilePos.x + 1, tilePos.y),
        new Vector2(tilePos.x - 1, tilePos.y),
        new Vector2(tilePos.x, tilePos.y + 1),
        new Vector2(tilePos.x, tilePos.y - 1)
    };

        foreach (Vector2 position in adjacentPositions)
        {
            PathTile tileAtPosition = Pad.Grid.GetTileAtPosition(position);

            //tile toevoegen wanneer deze bij het pad hoort
            if (Pad.RandomPath.Contains(tileAtPosition))
            {
                neighbors++;
            }
        }

        return neighbors;
    }

    public List<PathTile> GetNeighbors(PathTile currentTile) //buren van een tile vinden die geen obstakel zijn
    {
        Vector2 currentTilePos = Pad.Grid.GetTilePosition(currentTile);

        List<Vector2> adjacentPositions = new List<Vector2>
    {
        new Vector2(currentTilePos.x + 1, currentTilePos.y),
        new Vector2(currentTilePos.x - 1, currentTilePos.y),
        new Vector2(currentTilePos.x, currentTilePos.y + 1),
        new Vector2(currentTilePos.x, currentTilePos.y - 1)
    };

        List<PathTile> neighbors = new List<PathTile>();

        foreach (Vector2 position in adjacentPositions)
        {
            PathTile tileAtPosition = Pad.Grid.GetTileAtPosition(position);

            if (tileAtPosition != null && !tileAtPosition.IsObstacle)
            {
                neighbors.Add(tileAtPosition);
            }
        }

        return neighbors;
    }

    public float GetDistance(PathTile tileA, PathTile tileB) //hulpfunctie A Star
    {
        Vector2 posA = Pad.Grid.GetTilePosition(tileA);
        Vector2 posB = Pad.Grid.GetTilePosition(tileB);

        return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
    }

    public float GetHeuristic(PathTile currentTile, PathTile goalTile) //hulpfunctie A Star
    {
        Vector2 currentPos = Pad.Grid.GetTilePosition(currentTile);
        Vector2 goalPos = Pad.Grid.GetTilePosition(goalTile);

        return Mathf.Abs(currentPos.x - goalPos.x) + Mathf.Abs(currentPos.y - goalPos.y);
    }
}
