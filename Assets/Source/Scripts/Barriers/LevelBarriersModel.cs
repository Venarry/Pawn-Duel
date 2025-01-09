using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelBarriersModel
{
    private readonly List<BarrierData> _barriers = new();
    private int _rowCount;
    private int _columnsCount;

    public BarrierData[] Barriers => _barriers.ToArray();

    public event Action<Vector2Int, Vector2Int, Vector2Int, Vector2, Vector2, BarrierOrientration> ProjectionSet;

    public void InitLevel(int rowCount, int columnsCount)
    {
        _barriers.Clear();

        _rowCount = rowCount;
        _columnsCount = columnsCount;
    }

    public void Add(Vector2Int gridPosition, BarrierOrientration barrierOrientration)
    {
        if(barrierOrientration == BarrierOrientration.Horizontal)
        {
            Vector2 firstWall = new Vector2(gridPosition.x, gridPosition.y + 0.5f);
            Vector2 secondWall = new Vector2(gridPosition.x + 1, gridPosition.y + 0.5f);
        }

        //BarrierData barrierData = new();
        //_barriers.Add(barrierData);
    }

    public void TryAddProjection(Vector2Int gridPosition, BarrierOrientration barrierOrientration)
    {
        Vector2 firstWall;
        Vector2 secondWall;
        Vector2Int secondCellPosition;
        Vector2Int thirdCellPosition;

        if (barrierOrientration == BarrierOrientration.Vertical)
        {
            firstWall = new Vector2(gridPosition.x + 0.5f, gridPosition.y + 0.5f);
            secondWall = new Vector2(gridPosition.x + 1.5f, gridPosition.y + 0.5f);

            secondCellPosition = new Vector2Int(gridPosition.x, gridPosition.y + 1);
            thirdCellPosition = new Vector2Int(gridPosition.x + 1, gridPosition.y + 1);
        }
        else
        {
            firstWall = new Vector2(gridPosition.x + 0.5f, gridPosition.y + 0.5f);
            secondWall = new Vector2(gridPosition.x + 0.5f, gridPosition.y + 1.5f);

            secondCellPosition = new Vector2Int(gridPosition.x + 1, gridPosition.y);
            thirdCellPosition = new Vector2Int(gridPosition.x + 1, gridPosition.y + 1);
        }


        //Debug.Log($"{gridPosition} {firstWall} {secondWall}");

        ProjectionSet?.Invoke(gridPosition, secondCellPosition, thirdCellPosition, firstWall, secondWall, barrierOrientration);
    }
}
