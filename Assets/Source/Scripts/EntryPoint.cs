using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private LevelSpawner _levelSpawner;
    [SerializeField] private LevelCycleHandler _levelCycleHandler;
    [SerializeField] private LevelsDataSource _levelsDataSource;

    private void Awake()
    {
        _levelCycleHandler.Init(_levelSpawner);
        _levelCycleHandler.Enable();
        _levelSpawner.Init(_levelsDataSource.Levels);

        _levelSpawner.SpawnNextLevel();
    }

    private void OnDisable()
    {
        _levelCycleHandler.Disable();
    }
}
