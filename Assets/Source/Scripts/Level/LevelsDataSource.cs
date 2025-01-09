using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsDataSource : MonoBehaviour
{
    [SerializeField] private List<LevelData> _levelsData;

    public LevelData[] Levels => _levelsData.ToArray();
}
