using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] private Image _mainColor;

    public void SetPlayerColor()
    {
        _mainColor.color = GameSettings.PlayerColor;
    }

    public void SetEnemyColor()
    {
        _mainColor.color = GameSettings.EnemyColor;
    }
}
