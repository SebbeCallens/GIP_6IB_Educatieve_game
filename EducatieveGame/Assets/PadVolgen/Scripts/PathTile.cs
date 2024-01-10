using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer; //spriterenderer
    [SerializeField] private GameObject _mouseHighlight; //highlight
    [SerializeField] private GameObject _pathHighlight;
    private bool _isFinish = false; //of de tile de finish is
    private bool _isObstacle = false; //of de tile een obstakel is
    private bool _isLocation = false; //of de tile een locatie is
    private string _locationName = ""; //de naam van de locatie
    [SerializeField] private bool _visited = false;
    private PathManager _pathManager;

    public bool IsFinish { get => _isFinish; set => _isFinish = value; }
    public bool IsObstacle { get => _isObstacle; set => _isObstacle = value; }
    public bool IsLocation { get => _isLocation; set => _isLocation = value; }
    public bool Visited { get => _visited; set => _visited = value; }
    public SpriteRenderer Renderer { get => _renderer; set => _renderer = value; }
    public GameObject PathHighlight { get => _pathHighlight; set => _pathHighlight = value; }

    private void Awake() //highlight uitzetten
    {
        _mouseHighlight.SetActive(false);
        _pathManager = GameObject.Find("Path").GetComponent<PathManager>();
    }

    private void OnMouseEnter() //highlight aanzetten
    {
        _mouseHighlight.SetActive(true);
    }

    private void OnMouseExit() //highlight uitzetten
    {
        _mouseHighlight.SetActive(false);
    }

    private void OnMouseDown() //speler zoeken en hem proberen verplaatsen naar deze tile als dat mogelijk is
    {
        bool success = false;
        success = GameObject.FindWithTag("Player").GetComponent<Player>().TryMove(this);
        if (success)
        {
            Visited = true;
        }
        if(IsFinish && success)
        {
            // locaties overlopen
            bool everyLocationVisited = true;

            List<PathTile> checkpoints = GameObject.Find("Path").GetComponent<PathManager>().Checkpoints;
            foreach (PathTile checkpoint in checkpoints)
            {
                // kijken of de locatie bezocht is
                if (checkpoint.IsLocation && !checkpoint.Visited)
                {
                    everyLocationVisited = false;
                    break;
                }
            }
            // kijken of alle locaties bezocht zijn
            if (everyLocationVisited)
            {
                int aantalStappen = _pathManager.PlayerVisitedTiles.Count;
                int aStarLength = _pathManager.AStarSelectedTiles.Count;
                _pathManager.ShowPath(_pathManager.PlayerVisitedTiles, new Color(255, 0, 0, 0.5f));
                _pathManager.ShowPath(_pathManager.AStarSelectedTiles, new Color(0, 0, 255, 0.5f));
                // print info
                Debug.Log(aStarLength/(aantalStappen*1.0));
            }
        }
    }

    public void SetTile(Color color, bool isObstacle, bool isLocation, string locationName) //tile instellen met kleur, of het een obstakel is, of het een locatie is en de locatienaam
    {
        Renderer.color = color;
        IsObstacle = isObstacle;
        IsLocation = isLocation;
        _locationName = locationName;
    }
}
