using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCycleHandler : MonoBehaviour
{
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private Transform _characterParent;

    private GameObject _player;
    private LevelSpawner _levelSpawner;

    public void Init(LevelSpawner levelSpawner)
    {
        _levelSpawner = levelSpawner;
    }

    public void Enable()
    {
        _levelSpawner.LevelSpawned += OnLevelSpawn;
    }

    public void Disable()
    {
        _levelSpawner.LevelSpawned -= OnLevelSpawn;
    }

    private void OnLevelSpawn(Dictionary<Vector2Int, Cell> cells, int rowCount, int columnsCount)
    {
        Vector3 playerSpawnPosition = cells[new Vector2Int(rowCount - 1, (columnsCount - 1) / 2)].transform.position;
        Debug.Log(playerSpawnPosition);
        if(_player == null)
        {
            _player = Instantiate(_characterPrefab, playerSpawnPosition, Quaternion.identity, _characterParent);
        }
        else
        {
        }
            _player.GetComponent<RectTransform>().position = playerSpawnPosition;
    }

}