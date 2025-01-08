using UnityEngine;

public class VenarryGrid
{
    private readonly RectTransform _rectTransform;
    private readonly int _columnsCount;
    private readonly Vector2 _cellSize;
    private readonly Vector2 _spacing;
    private readonly RectOffset _padding;
    private readonly Alignment _alignment = Alignment.Center;

    public enum Alignment
    {
        Center,
        Left,
        Right,
        Top,
        Bottom,
    }

    public VenarryGrid(RectTransform rectTransform, int columnsCount, Vector2 cellSize, Vector2 spacing, RectOffset padding, Alignment alignment)
    {
        _rectTransform = rectTransform;
        _columnsCount = columnsCount;
        _cellSize = cellSize;
        _spacing = spacing;
        _padding = padding;
        _alignment = alignment;
    }

    public void AddElement(RectTransform newElement)
    {
        newElement.sizeDelta = _cellSize;
        newElement.SetParent(_rectTransform, false);
    }

    public void UpdatePositions()
    {
        int count = _rectTransform.childCount;

        float gridWidth = _columnsCount * (_cellSize.x + _spacing.x) - _spacing.x;
        int totalRows = Mathf.CeilToInt((float)count / _columnsCount);
        float gridHeight = totalRows * (_cellSize.y + _spacing.y) - _spacing.y;

        for (int i = 0; i < count; i++)
        {
            RectTransform child = _rectTransform.GetChild(i) as RectTransform;

            int row = i / _columnsCount;
            int column = i % _columnsCount;

            float x = column * (_cellSize.x + _spacing.x);
            float y = -row * (_cellSize.y + _spacing.y);

            switch (_alignment)
            {
                case Alignment.Center:
                    x -= gridWidth / 2 - _cellSize.x / 2;
                    y += gridHeight / 2 - _cellSize.y / 2;
                    break;
                case Alignment.Left:
                    y += gridHeight / 2 - _cellSize.y / 2;
                    break;
                case Alignment.Right:
                    x -= gridWidth - _cellSize.x;
                    y += gridHeight / 2 - _cellSize.y / 2;
                    break;
                case Alignment.Top:
                    x -= gridWidth / 2 - _cellSize.x / 2;
                    break;
                case Alignment.Bottom:
                    x -= gridWidth / 2 - _cellSize.x / 2;
                    y -= gridHeight - _cellSize.y;
                    break;
            }

            child.localPosition = new Vector2(x, y);
        }
    }

    public void UpdateContainerSize()
    {
        if (_alignment != Alignment.Center)
        {
            return;
        }

        int childCount = _rectTransform.childCount;

        int columnCount = Mathf.Max(1, _columnsCount);
        int rowCount = Mathf.CeilToInt((float)childCount / columnCount);

        float newWidth = _padding.left + _padding.right + columnCount * _cellSize.x + (columnCount - 1) * _spacing.x;
        float newHeight = _padding.top + _padding.bottom + rowCount * _cellSize.y + (rowCount - 1) * _spacing.y;

        _rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
    }
}
