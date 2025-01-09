using System.Collections.Generic;
using UnityEngine;

public class LevelCycleHandler : MonoBehaviour
{
    [SerializeField] private PlayerStepHandler _playerStepHandler;
    [SerializeField] private Character _characterPrefab;
    [SerializeField] private Transform _characterParent;
    [SerializeField] private Canvas _canvas;

    private Character _player;
    private Character _enemy;
    private CharacterFactory _characterFactory;
    private LevelSpawner _levelSpawner;
    private Dictionary<Vector2Int, Cell> _cells;

    public void Init(LevelSpawner levelSpawner)
    {
        _levelSpawner = levelSpawner;
        _characterFactory = new();
    }

    public void Enable()
    {
        _levelSpawner.LevelSpawned += OnLevelSpawn;
        _playerStepHandler.StepEnded += OnStepEnded;
    }

    public void Disable()
    {
        _levelSpawner.LevelSpawned -= OnLevelSpawn;
        _playerStepHandler.StepEnded -= OnStepEnded;
    }

    public void SetCharactersRaycastTarget(bool state)
    {
        _player.SetRaycastTarget(state);
        _enemy.SetRaycastTarget(state);
    }

    private void OnLevelSpawn(Dictionary<Vector2Int, Cell> cells, LevelData levelData)
    {
        _cells = cells;
        SpawnCharacters(levelData);
        _playerStepHandler.StartStep(_cells, _player);
    }

    private void SpawnCharacters(LevelData levelData)
    {
        //Vector3 playerSpawnPosition = cells[new Vector2Int(levelData.Size.y - 1, (levelData.Size.x - 1) / 2)].transform.position;
        //Vector3 enemySpawnPosition = cells[new Vector2Int(0, (levelData.Size.x - 1) / 2)].transform.position;

        Vector2Int playerSpawnCellIndex = new(levelData.PlayerSpawnPosition.y - 1, levelData.PlayerSpawnPosition.x - 1);
        Vector2Int enemySpawnCellIndex = new(levelData.EnemySpawnPosition.y - 1, levelData.EnemySpawnPosition.x - 1);

        Vector3 playerSpawnPosition = _cells[playerSpawnCellIndex].transform.position;
        Vector3 enemySpawnPosition = _cells[enemySpawnCellIndex].transform.position;

        if (_player == null)
        {
            _player = _characterFactory.Create(playerSpawnPosition, playerSpawnCellIndex, _canvas.transform, isPlayer: true);
            _enemy = _characterFactory.Create(enemySpawnPosition, enemySpawnCellIndex, _canvas.transform, isPlayer: false);
        }
        else
        {
            _player.SetPosition(playerSpawnPosition, playerSpawnCellIndex);
            _enemy.SetPosition(enemySpawnPosition, enemySpawnCellIndex);
        }
    }

    private void OnStepEnded()
    {
        _playerStepHandler.StartStep(_cells, _player);
    }
}
