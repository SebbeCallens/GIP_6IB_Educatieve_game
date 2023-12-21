using System.Collections.Generic;
using UnityEngine;

public class PathAStar : MonoBehaviour
{
    [SerializeField] private PathGrid _grid; //het grid
    [SerializeField] private PathManager _path; //het pad
    [SerializeField] private PathFunctions _pathFnc; //de functies van het pad

    public List<PathTile> FindShortestPath(PathTile startTile, PathTile endTile) //kortste pad vinden tussen 2 posities op het grid
    {
        PriorityQueue<PathTile> openSet = new PriorityQueue<PathTile>();
        Dictionary<PathTile, float> gScore = new Dictionary<PathTile, float>();
        Dictionary<PathTile, PathTile> cameFrom = new Dictionary<PathTile, PathTile>();

        openSet.Enqueue(startTile, 0);
        gScore[startTile] = 0;

        while (openSet.Count > 0)
        {
            PathTile currentTile = openSet.Dequeue();

            if (currentTile == endTile)
            {
                return ReconstructPath(cameFrom, currentTile);
            }

            foreach (PathTile neighbor in _pathFnc.GetNeighbors(currentTile))
            {
                float tentativeGScore = gScore[currentTile] + _pathFnc.GetDistance(currentTile, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    gScore[neighbor] = tentativeGScore;
                    float priority = tentativeGScore + _pathFnc.GetHeuristic(neighbor, endTile);
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
