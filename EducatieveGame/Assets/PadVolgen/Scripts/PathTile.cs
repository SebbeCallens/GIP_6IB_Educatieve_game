using UnityEngine;

public class PathTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer; //spriterenderer
    [SerializeField] private GameObject _highlight; //highlight
    private bool _isFinish = false; //of de tile de finish is
    private bool _isObstacle = false; //of de tile een obstakel is
    private bool _isLocation = false; //of de tile een locatie is
    private string _locationName = ""; //de naam van de locatie

    public bool IsFinish { get => _isFinish; set => _isFinish = value; }
    public bool IsObstacle { get => _isObstacle; set => _isObstacle = value; }
    public bool IsLocation { get => _isLocation; set => _isLocation = value; }

    private void Awake() //highlight uitzetten
    {
        _highlight.SetActive(false);
    }

    private void OnMouseEnter() //highlight aanzetten
    {
        _highlight.SetActive(true);
    }

    private void OnMouseExit() //highlight uitzetten
    {
        _highlight.SetActive(false);
    }

    private void OnMouseDown() //speler zoeken en hem proberen verplaatsen naar deze tile als dat mogelijk is
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().TryMove(this);
    }

    public void SetTile(Color color, bool isObstacle, bool isLocation, string locationName) //tile instellen met kleur, of het een obstakel is, of het een locatie is en de locatienaam
    {
        _renderer.color = color;
        IsObstacle = isObstacle;
        IsLocation = isLocation;
        _locationName = locationName;
    }
}
