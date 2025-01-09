using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private LevelSpawner _levelSpawner;
    [SerializeField] private LevelCycleHandler _levelCycleHandler;
    [SerializeField] private LevelsDataSource _levelsDataSource;
    [SerializeField] private BarriersView _barriersView;
    [SerializeField] private PlayerStepHandler _playerStepHandler;

    private void Awake()
    {
        LevelBarriersModel levelBarriersModel = new();

        _levelCycleHandler.Init(_levelSpawner);
        _levelCycleHandler.Enable();
        _levelSpawner.Init(levelBarriersModel, _levelsDataSource.Levels);
        _barriersView.Init(levelBarriersModel);
        _barriersView.Enable();
        _playerStepHandler.Init(levelBarriersModel);

        _levelSpawner.SpawnNextLevel();
    }

    private void OnDisable()
    {
        _levelCycleHandler.Disable();
        _barriersView.Disable();
    }
}
