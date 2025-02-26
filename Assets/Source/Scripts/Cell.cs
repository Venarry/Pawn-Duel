using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    [SerializeField] private Image _mainImage;
    [SerializeField] private Image _winImage;
    [SerializeField] private Color _highlightColor = Color.yellow;

    private LevelBarriersModel _levelBarriersModel;
    private PlayerStepHandler _playerStepHandler;
    private Color _startColor;
    private bool _isActive = false;
    
    public Vector2Int GridPosition { get; private set; } = Vector2Int.zero;
    public bool IsBlocked { get; private set; } = false;
    public bool IsPlayerWinCondition { get; private set; } = false;
    public bool IsEnemyWinCondition { get; private set; } = false;

    private void Awake()
    {
        _startColor = _mainImage.color;
    }

    public void SetWinCondition(bool isPlayerWinCell)
    {
        _winImage.enabled = true;

        if (isPlayerWinCell == true)
        {
            IsPlayerWinCondition = true;
            _winImage.color = GameSettings.PlayerColor;
        }
        else
        {
            IsEnemyWinCondition = true;
            _winImage.color = GameSettings.EnemyColor;
        }
    }

    public void Init(PlayerStepHandler playerStepHandler, LevelBarriersModel levelBarriersModel, Vector2Int position)
    {
        _levelBarriersModel = levelBarriersModel;
        _playerStepHandler = playerStepHandler;
        GridPosition = position;
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
        _isActive = true;
    }

    public void TurnOffCell()
    {
        if (IsBlocked == true)
        {
            return;
        }

        _mainImage.color = _startColor;
        _isActive = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out Character character) && _isActive == true)
        {
            character.SetPosition(transform.position, GridPosition);
        }

        if (eventData.pointerDrag.TryGetComponent(out BarrierButtonHandler barrierButtonHandler))
        {
            BarrierOrientration barrierOrientration = barrierButtonHandler.Orientration;
            
            if (barrierButtonHandler.HasBarriers == true)
            {
                if(_levelBarriersModel.TryAdd(GridPosition, barrierOrientration))
                {
                    _playerStepHandler.RefreshLights();
                    barrierButtonHandler.TryDecreaseBarrier();
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        if(eventData.pointerDrag.TryGetComponent(out BarrierButtonHandler barrierButtonHandler))
        {
            if (barrierButtonHandler.HasBarriers == true)
            {
                BarrierOrientration barrierOrientration = barrierButtonHandler.Orientration;
                _levelBarriersModel.TryApplyProjection(GridPosition, barrierOrientration);
            }
        }
    }
}
