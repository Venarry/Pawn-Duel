using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Character : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private Image _mainColor;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Transform _freeParent;
    private Vector3 _currentPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(Transform parent)
    {
        _freeParent = parent;
    }

    public void SetPosition(Vector3 postition)
    {
        _currentPosition = postition;
        transform.position = _currentPosition;
    }

    public void SetPlayerColor()
    {
        _mainColor.color = GameSettings.PlayerColor;
    }

    public void SetEnemyColor()
    {
        _mainColor.color = GameSettings.EnemyColor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        transform.position = _currentPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition +=
            eventData.delta / _freeParent.localScale.x;
    }
}
