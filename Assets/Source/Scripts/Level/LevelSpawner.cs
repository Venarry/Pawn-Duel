using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private RectTransform _grid;
    [SerializeField] private Cell _cellPrefab;

    private readonly Dictionary<Vector2Int, Cell> _cells = new();
    private Vector2 _cellSize = new(100, 100);
    private Vector2 _spacing = new(10, 10);
    private RectOffset _padding;
    private VenarryGrid _venarryGrid;
    private LevelData[] _levels;
    private int _activeLevelIndex = 0;

    public event Action<Dictionary<Vector2Int, Cell>, LevelData> LevelSpawned;

    public void Init(LevelData[] levels)
    {
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
        _venarryGrid = new(_grid, currentLevel.Size.x, _cellSize, _spacing, _padding, VenarryGrid.Alignment.Center);

        for (int i = 0; i < currentLevel.Size.y; i++)
        {
            for (int j = 0; j < currentLevel.Size.x; j++)
            {
                Cell cell = Instantiate(_cellPrefab, _grid.transform);
                cell.SetGridPosition(new Vector2Int(i, j));

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
