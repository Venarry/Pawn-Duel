using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image _mainImage;

    public Vector2Int Position { get; private set; } = Vector2Int.zero;
    public bool IsBlocked { get; private set; } = false;

    public void SetPosition(Vector2Int position)
    {
        Position = position;
    }

    public void Block()
    {
        Color blockColor = _mainImage.color;

        float darkMultiplier = 0.8f;
        blockColor *= darkMultiplier;

        _mainImage.color = blockColor;
        IsBlocked = true;
    }
}
