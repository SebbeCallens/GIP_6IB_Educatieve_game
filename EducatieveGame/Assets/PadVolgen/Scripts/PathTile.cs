using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRend; //spriterenderer
    [SerializeField] private GameObject _mouseHighlight; //highlight
    [SerializeField] private GameObject _pathHighlight; //highlight debug
    private bool _isFinish = false; //of de tile de finish is
    private bool _isObstacle = false; //of de tile een obstakel is
    private bool _isCheckpoint = false; //of de tile een locatie is
    private bool _visited = false; //of de tile al bezocht is
    private PathManager _pad; //het pad

    public SpriteRenderer SpriteRend { get => _spriteRend; set => _spriteRend = value; }
    public GameObject MouseHighlight { get => _mouseHighlight; set => _mouseHighlight = value; }
    public GameObject PathHighlight { get => _pathHighlight; set => _pathHighlight = value; }
    public bool IsFinish { get => _isFinish; set => _isFinish = value; }
    public bool IsObstacle { get => _isObstacle; set => _isObstacle = value; }
    public bool IsCheckpoint { get => _isCheckpoint; set => _isCheckpoint = value; }
    public bool Visited { get => _visited; private set => _visited = value; }
    private PathManager Pad { get => _pad; set => _pad = value; }

    private void Awake() //highlight uitzetten
    {
        Pad = GameObject.FindWithTag("GameView").GetComponent<PathManager>();
        MouseHighlight.SetActive(false);
    }

    private void OnMouseEnter() //highlight aanzetten
    {
        MouseHighlight.SetActive(true);
    }

    private void OnMouseExit() //highlight uitzetten
    {
        MouseHighlight.SetActive(false);
    }

    private void OnMouseDown() //speler zoeken en hem proberen verplaatsen naar deze tile als dat mogelijk is
    {
        bool success = GameObject.FindWithTag("Player").GetComponent<Player>().TryMove(this, Pad.Grid.GetTilePosition(this));

        if (!Pad.Generator.RandomOrder)
        {
            if (IsCheckpoint && success && !Visited && GetComponentInChildren<TextMeshPro>().text.Equals(GameObject.FindWithTag("Player").GetComponent<Player>().TargetLocation.ToString()))
            {
                Visited = true;
                GameObject.FindWithTag("Player").GetComponent<Player>().TargetLocation++;
                Destroy(transform.GetChild(transform.childCount - 2).gameObject);
            }
        }
        else
        {
            if (IsCheckpoint && success && !Visited)
            {
                Visited = true;
                GameObject.FindWithTag("Player").GetComponent<Player>().TargetLocation++;
                Destroy(transform.GetChild(transform.childCount - 2).gameObject);
                Pad.Checkpoints.Insert(Pad.Checkpoints.Count - 1, this);
            }
        }

        if(IsFinish && success)
        {
            bool everyLocationVisited = true;

            foreach (PathTile checkpoint in Pad.Checkpoints)
            {
                // kijken of de locatie bezocht is
                if (checkpoint.IsCheckpoint && !checkpoint.Visited)
                {
                    everyLocationVisited = false;
                    break;
                }
            }
            if (Pad.Generator.RandomOrder)
            {
                foreach (PathTile checkpoint in Pad.RandomPath)
                {
                    // kijken of de locatie bezocht is
                    if (checkpoint.IsCheckpoint && !checkpoint.Visited)
                    {
                        everyLocationVisited = false;
                        break;
                    }
                }
            }
            // kijken of alle locaties bezocht zijn
            if (everyLocationVisited)
            {
                Pad.Generator.ShowAStarPath(Color.red);
                if (Pad.Generator.Arrows && !Pad.Generator.ScrambledOrder)
                {
                    EndGame((Math.Round((double)Pad.RandomPath.Count / (GameObject.FindWithTag("Player").GetComponent<Player>().Steps + GameObject.FindWithTag("Player").GetComponent<Player>().WrongSteps), 2) * 100).ToString());
                }
                else
                {
                    EndGame((Math.Round((double)Pad.AStarPath.Count / (GameObject.FindWithTag("Player").GetComponent<Player>().Steps + GameObject.FindWithTag("Player").GetComponent<Player>().WrongSteps), 2) * 100).ToString());
                }
            }
        }
    }

    public void SetTile(Color color, bool isObstacle, bool isLocation) //tile instellen met kleur, of het een obstakel is, of het een locatie is en de locatienaam
    {
        SpriteRend.color = color;
        IsObstacle = isObstacle;
        IsCheckpoint = isLocation;
    }

    private void EndGame(string score)
    {
        EndScreenLogic.EndGame("PadVolgenMenu", "Pad volgen", score + "%", Camera.main.orthographicSize * 1.25f * 1.95f, Camera.main.transform.position, Camera.main.orthographicSize / 2.5f);
        GameObject gameview = GameObject.FindWithTag("GameView");
        gameview.transform.SetParent(null);
        gameview.transform.localScale = new(gameview.transform.localScale.x, gameview.transform.localScale.y, 1);
        DontDestroyOnLoad(gameview);
        SceneManager.LoadScene("EndScreen");
    }
}
