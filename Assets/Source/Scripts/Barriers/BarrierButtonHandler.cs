using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class BarrierButtonHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private TMP_Text _barriersCountLabel;
    [SerializeField] private GameObject _barrierPrefab;
    [SerializeField] private Transform _canvas;
    [SerializeField] private LevelCycleHandler _levelCycleHandler;

    private int _barriersCount;
    private RectTransform _rectTransform;
    private GameObject _activeBarrier;

    [field: SerializeField] public BarrierOrientration Orientration { get; private set; }
    public bool HasBarriers => _barriersCount > 0;

    public void SetLevelData(int barriersCount)
    {
        _barriersCount = barriersCount;
        _barriersCountLabel.text = _barriersCount.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_barriersCount <= 0)
        {
            return;
        }

        _levelCycleHandler.SetCharactersRaycastTarget(state: false);
        _activeBarrier = Instantiate(_barrierPrefab, _canvas);
        _rectTransform = _activeBarrier.GetComponent<RectTransform>();
        _rectTransform.position = eventData.pressPosition;
        //_activeBarrier.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public bool TryDecreaseBarrier()
    {
        if (_barriersCount <= 0)
        {
            return false;
        }

        _barriersCount--;
        _barriersCountLabel.text = _barriersCount.ToString();

        return true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_barriersCount <= 0)
        {
            return;
        }

        _rectTransform.anchoredPosition += eventData.delta / _canvas.localScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _levelCycleHandler.SetCharactersRaycastTarget(state: true);

        if (_activeBarrier != null)
        {
            Destroy(_activeBarrier);
            _activeBarrier = null;
        }
        //_activeBarrier.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
