using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private RectTransform _grid;
    [SerializeField] private PlayerStepHandler _playerStepHandler;
    [SerializeField] private Cell _cellPrefab;

    [SerializeField] private BarrierButtonHandler _horizontalBarriersButton;
    [SerializeField] private BarrierButtonHandler _verticalBarriersButton;

    private LevelBarriersModel _levelBarriersModel;
    private readonly Dictionary<Vector2Int, Cell> _cells = new();
    private RectOffset _padding;
    private VenarryGrid _venarryGrid;
    private LevelData[] _levels;
    private int _activeLevelIndex = 0;

    public Vector2 CellSize { get; private set; } = new(100, 100);
    public Vector2 Spacing { get; private set; } = new(10, 10);
    public Dictionary<Vector2Int, Cell> Cells => _cells.ToDictionary(c => c.Key, s => s.Value);

    public event Action<Dictionary<Vector2Int, Cell>, LevelData> LevelSpawned;

    public void Init(LevelBarriersModel levelBarriersModel, LevelData[] levels)
    {
        _levelBarriersModel = levelBarriersModel;
        _padding = new(10, 10, 10, 10);
        _levels = levels;
    }

    public void ResetLevels()
    {
        _activeLevelIndex = 0;
    }

    public void SpawnNextLevel()
    {
        if (_activeLevelIndex >= _levels.Length)
        {
            return;
        }

        LevelData currentLevel = _levels[_activeLevelIndex];
        _venarryGrid = new(_grid, currentLevel.Size.x, CellSize, Spacing, _padding, VenarryGrid.Alignment.Center);

        _horizontalBarriersButton.SetLevelData(currentLevel.HorizontalBarriersCount);
        _verticalBarriersButton.SetLevelData(currentLevel.VerticalBarriersCount);

        for (int i = 0; i < currentLevel.Size.y; i++)
        {
            for (int j = 0; j < currentLevel.Size.x; j++)
            {
                Cell cell = Instantiate(_cellPrefab, _grid.transform);
                cell.Init(_playerStepHandler, _levelBarriersModel, new Vector2Int(i, j));
                cell.gameObject.name = $"Cell({i}{j})";

                if(i == 0)
                {
                    cell.SetWinCondition(isPlayerWinCell: true);
                }
                
                if(i == currentLevel.Size.y - 1)
                {
                    cell.SetWinCondition(isPlayerWinCell: false);
                }

                if (currentLevel.BlockedCells.Contains(new Vector2Int(j + 1, i + 1)))
                {
                    cell.Block();
                }

                _venarryGrid.AddElement(cell.GetComponent<RectTransform>());

                _cells.Add(new Vector2Int(i, j), cell);
            }
        }

        _venarryGrid.UpdatePositions();
        _venarryGrid.UpdateContainerSize();

        _activeLevelIndex++;

        LevelSpawned?.Invoke(_cells.ToDictionary(c => c.Key, s => s.Value), currentLevel);
    }
}
