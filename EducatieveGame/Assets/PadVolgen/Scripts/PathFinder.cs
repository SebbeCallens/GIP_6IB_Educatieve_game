using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private PathManager _pad;
    [SerializeField] private GameObject _arrow;
    private PathTile _nextTile;

    private PathManager Pad { get => _pad; set => _pad = value; }
    private GameObject Arrow { get => _arrow; set => _arrow = value; }
    public PathTile NextTile { get => _nextTile; private set => _nextTile = value; }

    public void ShowArrow()
    {
        Arrow.SetActive(true);
        ShowNextArrow();
    }

    public void ShowNextArrow()
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        List<PathTile> tiles;
        if (Pad.Generator.ScrambledOrder)
        {
            tiles = Pad.AStarPath;
        }
        else
        {
            tiles = Pad.RandomPath;
        }
        if (player.Steps < tiles.Count)
        {
            Vector2 nextTilePosition = Pad.Grid.GetTilePosition(tiles[player.Steps]);
            NextTile = Pad.Grid.GetTileAtPosition(nextTilePosition);
            Vector2 direction = (nextTilePosition - player.CurrentPosition).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            Arrow.SetActive(false);
        }
    }
}
