using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerStepHandler : MonoBehaviour
{
    [SerializeField] private GameObject _barrierButtonsParent;
    private Cell[] _activeAvailableWays;
    private Character _activeCharacter;
    private LevelBarriersModel _levelBarriersModel;
    private Dictionary<Vector2Int, Cell> _levelCells;

    public event Action StepEnded;

    public void Init(LevelBarriersModel levelBarriersModel)
    {
        _levelBarriersModel = levelBarriersModel;
    }

    public void StartStep(Dictionary<Vector2Int, Cell> levelCells, Character player)
    {
        _levelCells = levelCells;
        _activeCharacter = player;
        _barrierButtonsParent.SetActive(true);

        _activeAvailableWays = GetAvailableCellsForMove(_levelCells, _activeCharacter.GridPosition);

        foreach (Cell cell in _activeAvailableWays)
        {
            cell.HighlightCell();
        }

        _activeCharacter.PositionChanged += OnPositionChange;
    }

    public void RefreshLights()
    {
        foreach (Cell cell in _activeAvailableWays)
        {
            cell.TurnOffCell();
        }

        _activeAvailableWays = GetAvailableCellsForMove(_levelCells, _activeCharacter.GridPosition);

        foreach (Cell cell in _activeAvailableWays)
        {
            cell.HighlightCell();
        }
    }

    private void OnPositionChange()
    {
        EndStep();
    }

    private void EndStep()
    {
        foreach (Cell cell in _activeAvailableWays)
        {
            cell.TurnOffCell();
        }

        _barrierButtonsParent.SetActive(false);
        _activeCharacter.PositionChanged -= OnPositionChange;
        StepEnded?.Invoke();
    }

    private Cell[] GetAvailableCellsForMove(Dictionary<Vector2Int, Cell> source, Vector2Int gridPosition)
    {
        List<Cell> availableCells = new();
        Vector2[] barriers = _levelBarriersModel.Barriers;

        foreach (Vector2 item in barriers)
        {
            Debug.Log(item);
        }

        foreach (KeyValuePair<Vector2Int, Cell> cell in source)
        {
            if (cell.Value.IsBlocked)
            {
                continue;
            }

            Vector2Int currentCellPosition = cell.Key;

            if (gridPosition == currentCellPosition)
            {
                continue;
            }

            int moveDistance = 1;

            if (gridPosition.x != currentCellPosition.x && gridPosition.y != currentCellPosition.y)
            {
                continue;
            }

            if (Mathf.Abs(gridPosition.y - currentCellPosition.y) > moveDistance)
            {
                continue;
            }

            if (Mathf.Abs(gridPosition.x - currentCellPosition.x) > moveDistance)
            {
                continue;
            }

            if(barriers.Contains(((Vector2)gridPosition + (Vector2)currentCellPosition) / 2f) == true)
            {
                continue;
            }

            availableCells.Add(cell.Value);
        }

        return availableCells.ToArray();
    }
}
