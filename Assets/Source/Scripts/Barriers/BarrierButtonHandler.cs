using UnityEngine;
using UnityEngine.EventSystems;

public partial class BarrierButtonHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private GameObject _barrierPrefab;
    [SerializeField] private Transform _canvas;

    [field: SerializeField] public BarrierOrientration Orientration { get; private set; }

    private GameObject _activeBarrier;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _activeBarrier = Instantiate(_barrierPrefab, _canvas);
        _activeBarrier.GetComponent<RectTransform>().position = eventData.pressPosition;
        _activeBarrier.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _activeBarrier.GetComponent<RectTransform>().anchoredPosition += eventData.delta / _canvas.localScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _activeBarrier.GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(_activeBarrier);
    }
}
