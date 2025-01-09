using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image _mainImage;
    [SerializeField] private Color _highlightColor = Color.yellow;

    private Color _startColor;

    public Vector2Int Position { get; private set; } = Vector2Int.zero;
    public bool IsBlocked { get; private set; } = false;

    private void Awake()
    {
        _startColor = _mainImage.color;
    }

    public void SetPosition(Vector2Int position)
    {
        Position = position;
    }

    public void Block()
    {
        if (IsBlocked == true)
        {
            return;
        }

        Color blockColor = _startColor;

        float darkMultiplier = 0.8f;
        blockColor *= darkMultiplier;

        _mainImage.color = blockColor;
        IsBlocked = true;
    }

    public void HighlightCell()
    {
        if (IsBlocked == true)
        {
            return;
        }

        _mainImage.color = _highlightColor;
    }

    public void TurnOffCell()
    {
        if (IsBlocked == true)
        {
            return;
        }

        _mainImage.color = _startColor;
    }
}
