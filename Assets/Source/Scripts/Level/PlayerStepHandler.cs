using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStepHandler : MonoBehaviour
{
    [SerializeField] private GameObject _barrierButtonsParent;
    private Cell[] _activeAvailableWays;
    private Character _activeCharacter;

    public event Action StepEnded;

    public void StartStep(Dictionary<Vector2Int, Cell> levelCells, Character player)
    {
        _activeCharacter = player;
        _barrierButtonsParent.SetActive(true);

        _activeAvailableWays = GetAvailableCellsForMove(levelCells, player.GridPosition);

        foreach (Cell cell in _activeAvailableWays)
        {
            cell.HighlightCell();
        }

        _activeCharacter.PositionChanged += OnPositionChange;
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

            //Debug.Log($"{movePosition} {currentCellPosition}; {movePosition.x - currentCellPosition.x} {movePosition.y - currentCellPosition.y}");
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

            availableCells.Add(cell.Value);
        }

        return availableCells.ToArray();
    }
}
