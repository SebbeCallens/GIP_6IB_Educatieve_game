using System.Collections.Generic;
using UnityEngine;

public class PathAStar : MonoBehaviour
{
    [SerializeField] private PathManager _pad;  //het pad

    private PathManager Pad { get => _pad; set => _pad = value; }

    public List<PathTile> FindShortestPath(PathTile startTile, PathTile endTile) //kortste pad vinden tussen 2 posities op het grid
    {
        PriorityQueue<PathTile> openSet = new();
        Dictionary<PathTile, float> gScore = new();
        Dictionary<PathTile, PathTile> cameFrom = new();

        openSet.Enqueue(startTile, 0);
        gScore[startTile] = 0;

        while (openSet.Count > 0)
        {
            PathTile currentTile = openSet.Dequeue();

            if (currentTile == endTile)
            {
                return ReconstructPath(cameFrom, currentTile);
            }

            foreach (PathTile neighbor in Pad.Functions.GetNeighbors(currentTile))
            {
                float tentativeGScore = gScore[currentTile] + Pad.Functions.GetDistance(currentTile, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    gScore[neighbor] = tentativeGScore;
                    float priority = tentativeGScore + Pad.Functions.GetHeuristic(neighbor, endTile);
                    openSet.Enqueue(neighbor, priority);
                    cameFrom[neighbor] = currentTile;
                }
            }
        }

        return null;
    }

    private List<PathTile> ReconstructPath(Dictionary<PathTile, PathTile> cameFrom, PathTile current) //hulpfunctie A Star
    {
        List<PathTile> path = new List<PathTile> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }

        return path;
    }
}
