using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileScript : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isWalkable;

    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null;

    void Update()
    {
        if(OccupiedUnit != null)
        {
            if (OccupiedUnit.Faction == Faction.Player) UnitManager.Instance.SetSelectedPlayer((BasePlayer)OccupiedUnit);
        }
    }

    public virtual void Init(int x, int y)
    {
        
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    void OnMouseDown()
    {
        if(OccupiedUnit != null)
        {
            if (UnitManager.Instance.SelectedPlayer != null)
            {
                if (((UnitManager.Instance.SelectedPlayer.transform.position.x == transform.position.x + 1 || UnitManager.Instance.SelectedPlayer.transform.position.x == transform.position.x - 1) && UnitManager.Instance.SelectedPlayer.transform.position.y == transform.position.y) || ((UnitManager.Instance.SelectedPlayer.transform.position.y == transform.position.y + 1 || UnitManager.Instance.SelectedPlayer.transform.position.y == transform.position.y - 1) && UnitManager.Instance.SelectedPlayer.transform.position.x == transform.position.x))
                {
                    var finish = (BaseFinish)OccupiedUnit;
                    Destroy(finish.gameObject);
                    SetUnit(UnitManager.Instance.SelectedPlayer);
                }
            }
        }

        if(OccupiedUnit == null && _isWalkable)
        {
            if(UnitManager.Instance.SelectedPlayer != null)
            {
                if (((UnitManager.Instance.SelectedPlayer.transform.position.x == transform.position.x + 1 || UnitManager.Instance.SelectedPlayer.transform.position.x == transform.position.x - 1) && UnitManager.Instance.SelectedPlayer.transform.position.y == transform.position.y) || ((UnitManager.Instance.SelectedPlayer.transform.position.y == transform.position.y + 1 || UnitManager.Instance.SelectedPlayer.transform.position.y == transform.position.y - 1) && UnitManager.Instance.SelectedPlayer.transform.position.x == transform.position.x))
                {
                    SetUnit(UnitManager.Instance.SelectedPlayer);
                }
                
            }
        }
    }

    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }
}
