using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    private List<ScriptableUnit> _units;

    public BasePlayer SelectedPlayer;

    void Awake()
    {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void SpawnPlayer()
    {
        var randomPrefab = GetRandomUnit<BasePlayer>(Faction.Player);
        var spawnedPlayer = Instantiate(randomPrefab);
        var randomSpawnTile = GridManager.Instance.GetPlayerSpawnTile();

        randomSpawnTile.SetUnit(spawnedPlayer);

        GameManager.Instance.ChangeState(GameState.SpawnFinish);
    }

    public void SpawnFinish()
    {
        var randomPrefab = GetRandomUnit<BaseFinish>(Faction.Finish);
        var spawnedFinish = Instantiate(randomPrefab);
        var randomSpawnTile = GridManager.Instance.GetFinishSpawnTile();

        randomSpawnTile.SetUnit(spawnedFinish);
    }

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o=>Random.value).First().UnitPrefab;
    }

    public void SetSelectedPlayer(BasePlayer player)
    {
        SelectedPlayer = player;
    }
}
