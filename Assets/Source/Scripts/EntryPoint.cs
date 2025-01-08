using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private LevelSpawner _levelSpawner;
    [SerializeField] private LevelCycleHandler _levelCycleHandler;

    private void Awake()
    {
        _levelCycleHandler.Init(_levelSpawner);
        _levelCycleHandler.Enable();
        _levelSpawner.Init();

        _levelSpawner.SpawnLevel();
    }

    private void OnDisable()
    {
        _levelCycleHandler.Disable();
    }
}
