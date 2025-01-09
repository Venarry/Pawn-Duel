using System;
using System.Collections.Generic;
using UnityEngine;

public class BarriersView : MonoBehaviour
{
    [SerializeField] private LevelSpawner _levelSpawner;
    [SerializeField] private GameObject _projectionPrefab;
    [SerializeField] private Transform _canvas;

    private LevelBarriersModel _barriersModel;
    private List<GameObject> _activeBarriers = new();
    private List<GameObject> _activeProjections = new();

    public void Init(LevelBarriersModel levelBarriersModel)
    {
        _barriersModel = levelBarriersModel;
    }

    public void Enable()
    {
        _barriersModel.ProjectionSet += OnProjectionSet;
        _barriersModel.BarrierAdd += OnBarrierAdd;
    }

    public void Disable()
    {
        _barriersModel.ProjectionSet -= OnProjectionSet;
        _barriersModel.BarrierAdd -= OnBarrierAdd;
    }

    private void OnBarrierAdd(
        Vector2Int sourceCellPosition,
        Vector2Int secondCellPosition,
        Vector2Int thirdCellPosition,
        Vector2 firstWall,
        Vector2 secondWall,
        BarrierOrientration barrierOrientration)
    {
        TryRemoveProjection();

        TryCreateBarrier(sourceCellPosition, secondCellPosition, thirdCellPosition, barrierOrientration, _activeBarriers);
    }

    private void OnProjectionSet(
        Vector2Int sourceCellPosition,
        Vector2Int secondCellPosition,
        Vector2Int thirdCellPosition,
        Vector2 firstWall,
        Vector2 secondWall,
        BarrierOrientration barrierOrientration)
    {
        TryRemoveProjection();

        TryCreateBarrier(sourceCellPosition, secondCellPosition, thirdCellPosition, barrierOrientration, _activeProjections);
        /*Dictionary<Vector2Int, Cell> cells = _levelSpawner.Cells;

        if (cells.ContainsKey(secondCellPosition) == false || cells.ContainsKey(thirdCellPosition) == false)
            return;

        Cell selectedCell = cells[sourceCellPosition];
        Cell secondCell = cells[secondCellPosition];
        Cell thirdCell = cells[thirdCellPosition];

        Vector3 firstBarrierGlobalPosition = (selectedCell.transform.position + secondCell.transform.position) / 2;
        GameObject firstBarrier = Instantiate(_projectionPrefab, firstBarrierGlobalPosition, Quaternion.identity, _canvas);

        Vector3 secondBarrierGlobalPosition = firstBarrierGlobalPosition;

        Vector2 barrierSize;

        if (barrierOrientration == BarrierOrientration.Horizontal)
        {
            secondBarrierGlobalPosition.x = (thirdCell.transform.position.x + thirdCell.transform.position.x) / 2;
            barrierSize = new Vector2(_levelSpawner.CellSize.x, _levelSpawner.Spacing.y);
        }
        else
        {
            secondBarrierGlobalPosition.y = (thirdCell.transform.position.y + thirdCell.transform.position.y) / 2;
            barrierSize = new Vector2(_levelSpawner.CellSize.y, _levelSpawner.Spacing.x);
        }

        GameObject secondBarrier = Instantiate(_projectionPrefab, secondBarrierGlobalPosition, Quaternion.identity, _canvas);

        firstBarrier.GetComponent<RectTransform>().sizeDelta = barrierSize;
        secondBarrier.GetComponent<RectTransform>().sizeDelta = barrierSize;

        _activeProjections.Add(firstBarrier);
        _activeProjections.Add(secondBarrier);*/
    }

    private void TryRemoveProjection()
    {
        if (_activeProjections.Count > 0)
        {
            foreach (GameObject projection in _activeProjections)
            {
                Destroy(projection);
            }

            _activeProjections.Clear();
        }
    }

    private void TryCreateBarrier(
        Vector2Int sourceCellPosition,
        Vector2Int secondCellPosition,
        Vector2Int thirdCellPosition,
        BarrierOrientration barrierOrientration,
        List<GameObject> barriersCollection)
    {
        Dictionary<Vector2Int, Cell> cells = _levelSpawner.Cells;

        if (cells.ContainsKey(secondCellPosition) == false || cells.ContainsKey(thirdCellPosition) == false)
            return;

        Cell selectedCell = cells[sourceCellPosition];
        Cell secondCell = cells[secondCellPosition];
        Cell thirdCell = cells[thirdCellPosition];

        Vector3 firstBarrierGlobalPosition = (selectedCell.transform.position + secondCell.transform.position) / 2;
        GameObject firstBarrier = Instantiate(_projectionPrefab, firstBarrierGlobalPosition, Quaternion.identity, _canvas);

        Vector3 secondBarrierGlobalPosition = firstBarrierGlobalPosition;

        Vector2 barrierSize;

        if (barrierOrientration == BarrierOrientration.Horizontal)
        {
            secondBarrierGlobalPosition.x = (thirdCell.transform.position.x + thirdCell.transform.position.x) / 2;
            barrierSize = new Vector2(_levelSpawner.CellSize.x, _levelSpawner.Spacing.y);
        }
        else
        {
            secondBarrierGlobalPosition.y = (thirdCell.transform.position.y + thirdCell.transform.position.y) / 2;
            barrierSize = new Vector2(_levelSpawner.Spacing.y, _levelSpawner.CellSize.x);
        }

        GameObject secondBarrier = Instantiate(_projectionPrefab, secondBarrierGlobalPosition, Quaternion.identity, _canvas);

        firstBarrier.GetComponent<RectTransform>().sizeDelta = barrierSize;
        secondBarrier.GetComponent<RectTransform>().sizeDelta = barrierSize;

        barriersCollection.Add(firstBarrier);
        barriersCollection.Add(secondBarrier);
    }
}
