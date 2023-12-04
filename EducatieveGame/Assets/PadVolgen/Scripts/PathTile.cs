using UnityEngine;

public class PathTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    private bool _hasPlayer = false;
    private bool _isFinish = false;
    private bool _isObstacle = false;
    private bool _isLocation = false;
    private string _locationName = "";

    public bool HasPlayer { get => _hasPlayer; set => _hasPlayer = value; }
    public bool IsFinish { get => _isFinish; set => _isFinish = value; }

    private void Awake()
    {
        _highlight.SetActive(false);
    }

    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    private void OnMouseDown()
    {

    }

    public void SetTile(Color color, bool isObstacle, bool isLocation, string locationName)
    {
        _renderer.color = color;
        _isObstacle = isObstacle;
        _isLocation = isLocation;
        _locationName = locationName;
    }
}
