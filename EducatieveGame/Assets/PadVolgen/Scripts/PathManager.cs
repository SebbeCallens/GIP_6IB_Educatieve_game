using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private PathFunctions _functions;
    [SerializeField] private PathAStar _aStar;
    [SerializeField] private PathGenerator _generator;
    [SerializeField] private PathGrid _grid;
    [SerializeField] private PathFinder _finder;
    [SerializeField] private int _locations;
    private List<PathTile> _randomPath = new(); //lijst met tiles van het pad
    private List<PathTile> _AStarPath = new(); //lijst met tiles van het A Star pad
    private List<PathTile> _checkpoints = new(); //tiles start, locaties en einde

    public PathFunctions Functions { get => _functions; private set => _functions = value; }
    public PathAStar AStar { get => _aStar; private set => _aStar = value; }
    public PathGenerator Generator { get => _generator; private set => _generator = value; }
    public PathGrid Grid { get => _grid; private set => _grid = value; }
    public PathFinder Finder { get => _finder; private set => _finder = value; }
    public int Locations { get => _locations; set => _locations = value; }
    public List<PathTile> RandomPath { get => _randomPath; set => _randomPath = value; }
    public List<PathTile> AStarPath { get => _AStarPath; set => _AStarPath = value; }
    public List<PathTile> Checkpoints { get => _checkpoints; set => _checkpoints = value; }

    private void Start()
    {
        Grid.GenerateGrid();
        Generator.GeneratePath(Generator.SpawnPlayer(), PlayerPrefs.GetInt("puzzeldifficulty"));
    }
}
