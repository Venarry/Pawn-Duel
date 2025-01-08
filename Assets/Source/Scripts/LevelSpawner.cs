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
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnsCount;

    private readonly Dictionary<Vector2Int, Cell> _cells = new();
    private Vector2 _cellSize = new(100, 100);
    private Vector2 _spacing = new(10, 10);
    private RectOffset _padding;
    private VenarryGrid _venarryGrid;

    public event Action<Dictionary<Vector2Int, Cell>, int, int> LevelSpawned;

    public void Init()
    {
        _padding = new(10, 10, 10, 10);
        _venarryGrid = new(_grid, _columnsCount, _cellSize, _spacing, _padding, VenarryGrid.Alignment.Center);
    }

    public void SpawnLevel()
    {
        for (int i = 0; i < _rowCount; i++)
        {
            for (int j = 0; j < _columnsCount; j++)
            {
                Cell cell = Instantiate(_cellPrefab, _grid.transform);
                //cell.Block();
                _venarryGrid.AddElement(cell.GetComponent<RectTransform>());

                _cells.Add(new Vector2Int(i, j), cell);
            }
        }

        _venarryGrid.UpdatePositions();
        _venarryGrid.UpdateContainerSize();

        LevelSpawned?.Invoke(_cells.ToDictionary(c => c.Key, s => s.Value), _rowCount, _columnsCount);
    }
}
