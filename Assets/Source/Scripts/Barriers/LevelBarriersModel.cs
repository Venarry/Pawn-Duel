using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelBarriersModel
{
    private readonly List<Vector2> _barriers = new();
    private int _rowCount;
    private int _columnsCount;

    public Vector2[] Barriers => _barriers.ToArray();

    public event Action<Vector2Int, Vector2Int, Vector2Int, Vector2, Vector2, BarrierOrientration> ProjectionSet;
    public event Action<Vector2Int, Vector2Int, Vector2Int, Vector2, Vector2, BarrierOrientration> BarrierAdd;

    public void InitLevel(int rowCount, int columnsCount)
    {
        _barriers.Clear();

        _rowCount = rowCount;
        _columnsCount = columnsCount;
    }

    public bool TryAdd(Vector2Int gridPosition, BarrierOrientration barrierOrientration)
    {
        if(ApplyWalls(
            gridPosition, barrierOrientration,
            out Vector2 firstWall,
            out Vector2 secondWall,
            out Vector2Int secondCellPosition,
            out Vector2Int thirdCellPosition))
        {
            //BarrierData barrierData = new(firstWall, secondWall);
            _barriers.Add(firstWall);
            _barriers.Add(secondWall);

            BarrierAdd?.Invoke(gridPosition, secondCellPosition, thirdCellPosition, firstWall, secondWall, barrierOrientration);

            return true;
        }

        return false;
    }

    public void TryApplyProjection(Vector2Int gridPosition, BarrierOrientration barrierOrientration)
    {
        if(ApplyWalls(
            gridPosition, barrierOrientration,
            out Vector2 firstWall,
            out Vector2 secondWall,
            out Vector2Int secondCellPosition,
            out Vector2Int thirdCellPosition))
        {
            ProjectionSet?.Invoke(gridPosition, secondCellPosition, thirdCellPosition, firstWall, secondWall, barrierOrientration);
        }
    }

    private bool ApplyWalls(
        Vector2Int gridPosition,
        BarrierOrientration barrierOrientration,
        out Vector2 firstWall,
        out Vector2 secondWall,
        out Vector2Int secondCellPosition,
        out Vector2Int thirdCellPosition)
    {
        if (barrierOrientration == BarrierOrientration.Vertical)
        {
            firstWall = new Vector2(gridPosition.x + 0f, gridPosition.y + 0.5f);
            secondWall = new Vector2(gridPosition.x + 1f, gridPosition.y + 0.5f);

            secondCellPosition = new Vector2Int(gridPosition.x, gridPosition.y + 1);
            thirdCellPosition = new Vector2Int(gridPosition.x + 1, gridPosition.y + 1);
        }
        else
        {
            firstWall = new Vector2(gridPosition.x + 0.5f, gridPosition.y + 0f);
            secondWall = new Vector2(gridPosition.x + 0.5f, gridPosition.y + 1f);

            secondCellPosition = new Vector2Int(gridPosition.x + 1, gridPosition.y);
            thirdCellPosition = new Vector2Int(gridPosition.x + 1, gridPosition.y + 1);
        }

        if (gridPosition.x >= _rowCount - 1 || gridPosition.y >= _columnsCount - 1)
            return false;

        return true;
    }
}
