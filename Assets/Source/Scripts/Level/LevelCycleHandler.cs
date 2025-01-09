using System.Collections.Generic;
using UnityEngine;

public class LevelCycleHandler : MonoBehaviour
{
    [SerializeField] private Character _characterPrefab;
    [SerializeField] private Transform _characterParent;
    [SerializeField] private Canvas _canvas;

    private Character _player;
    private Character _enemy;

    private CharacterFactory _characterFactory;

    private LevelSpawner _levelSpawner;

    public void Init(LevelSpawner levelSpawner)
    {
        _levelSpawner = levelSpawner;
        _characterFactory = new();
    }

    public void Enable()
    {
        _levelSpawner.LevelSpawned += OnLevelSpawn;
    }

    public void Disable()
    {
        _levelSpawner.LevelSpawned -= OnLevelSpawn;
    }

    private void OnLevelSpawn(Dictionary<Vector2Int, Cell> cells, LevelData levelData)
    {
        //Vector3 playerSpawnPosition = cells[new Vector2Int(levelData.Size.y - 1, (levelData.Size.x - 1) / 2)].transform.position;
        //Vector3 enemySpawnPosition = cells[new Vector2Int(0, (levelData.Size.x - 1) / 2)].transform.position;

        Vector2Int playerSpawnCellIndex = new Vector2Int(levelData.PlayerSpawnPosition.y - 1, levelData.PlayerSpawnPosition.x - 1);
        Vector2Int enemySpawnCellIndex = new Vector2Int(levelData.EnemySpawnPosition.y - 1, levelData.EnemySpawnPosition.x - 1);

        Vector3 playerSpawnPosition = cells[playerSpawnCellIndex].transform.position;
        Vector3 enemySpawnPosition = cells[enemySpawnCellIndex].transform.position;

        if (_player == null)
        {
            _player = _characterFactory.Create(playerSpawnPosition, playerSpawnCellIndex, _canvas.transform, isPlayerColor: true);
            _enemy = _characterFactory.Create(enemySpawnPosition, enemySpawnCellIndex, _canvas.transform, isPlayerColor: false);
        }
        else
        {
            _player.transform.position = playerSpawnPosition;
            _enemy.transform.position = enemySpawnPosition;
        }

        foreach (Cell item in GetAvailableCellsForMove(cells, _player.GridPosition))
        {
            item.HighlightCell();
        } 
    }

    private Cell[] GetAvailableCellsForMove(Dictionary<Vector2Int, Cell> source, Vector2Int movePosition)
    {
        List<Cell> availableCells = new();

        foreach (KeyValuePair<Vector2Int, Cell> cell in source)
        {
            if(cell.Value.IsBlocked)
            {
                continue;
            }

            Vector2Int currentCellPosition = cell.Key;

            if(movePosition == currentCellPosition)
            {
                continue;
            }

            //Debug.Log($"{movePosition} {currentCellPosition}; {movePosition.x - currentCellPosition.x} {movePosition.y - currentCellPosition.y}");
            int moveDistance = 1;

            if(movePosition.x != currentCellPosition.x && movePosition.y != currentCellPosition.y)
            {
                continue;
            }

            if(Mathf.Abs(movePosition.y - currentCellPosition.y) > moveDistance)
            {
                continue;
            }

            if (Mathf.Abs(movePosition.x - currentCellPosition.x) > moveDistance)
            {
                continue;
            }

            availableCells.Add(cell.Value);
        }

        return availableCells.ToArray();
    }
}
