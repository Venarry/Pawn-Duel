using System;
using System.Collections.Generic;
using UnityEngine;

public class BarriersView : MonoBehaviour
{
    [SerializeField] private LevelSpawner _levelSpawner;
    [SerializeField] private GameObject _projectionPrefab;
    [SerializeField] private Transform _canvas;

    private LevelBarriersModel _barriersModel;
    private List<GameObject> _activeProjections = new();

    public void Init(LevelBarriersModel levelBarriersModel)
    {
        _barriersModel = levelBarriersModel;
    }

    public void Enable()
    {
        _barriersModel.ProjectionSet += OnProjectionSet;
    }

    public void Disable()
    {
        _barriersModel.ProjectionSet -= OnProjectionSet;
    }

    private void OnProjectionSet(
        Vector2Int sourceCellPosition,
        Vector2Int secondCellPosition,
        Vector2Int thirdCellPosition,
        Vector2 firstWall,
        Vector2 secondWall,
        BarrierOrientration barrierOrientration)
    {
        Dictionary<Vector2Int, Cell> cells = _levelSpawner.Cells;

        if(_activeProjections.Count > 0)
        {
            foreach (GameObject projection in _activeProjections)
            {
                Destroy(projection);
            }

            _activeProjections.Clear();
        }

        if (cells.ContainsKey(secondCellPosition) == false || cells.ContainsKey(thirdCellPosition) == false)
            return;

        Cell selectedCell = cells[sourceCellPosition];
        Cell secondCell = cells[secondCellPosition];
        Cell thirdCell = cells[thirdCellPosition];

        Vector3 firstBarrierGlobalPosition = (selectedCell.transform.position + secondCell.transform.position) / 2;
        GameObject firstBarrier = Instantiate(_projectionPrefab, firstBarrierGlobalPosition, Quaternion.identity, _canvas);

        Vector3 secondBarrierGlobalPosition = firstBarrierGlobalPosition;

        if (barrierOrientration == BarrierOrientration.Horizontal)
        {
            secondBarrierGlobalPosition.x = (thirdCell.transform.position.x + thirdCell.transform.position.x) / 2;
        }
        else
        {
            secondBarrierGlobalPosition.y = (thirdCell.transform.position.y + thirdCell.transform.position.y) / 2;
        }

        GameObject secondBarrier = Instantiate(_projectionPrefab, secondBarrierGlobalPosition, Quaternion.identity, _canvas);

        _activeProjections.Add(firstBarrier);
        _activeProjections.Add(secondBarrier);
    }
}
