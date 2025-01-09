using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public Vector2Int Size = new(7, 7);
    public List<Vector2Int> BlockedCells;
    public Vector2Int PlayerSpawnPosition;
    public Vector2Int EnemySpawnPosition;
}
