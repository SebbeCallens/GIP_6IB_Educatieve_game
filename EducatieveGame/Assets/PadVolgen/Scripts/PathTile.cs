using UnityEngine;

public class PathTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    private bool _hasPlayer;
    private bool _isObstacle;
    private bool _isLocation;
    private string _locationName;

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
