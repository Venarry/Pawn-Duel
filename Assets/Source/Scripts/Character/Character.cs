using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Character : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private Image _mainColor;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Transform _freeParent;
    private Vector3 _currentGlobalPosition;
    private bool _isPlayer = false;

    public Vector2Int GridPosition { get; private set; }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(Transform parent, bool isPlayer)
    {
        _freeParent = parent;
        _isPlayer = isPlayer;

        if(_isPlayer == true)
        {
            SetPlayerColor();
        }
        else
        {
            SetEnemyColor();
        }
    }

    public void SetPosition(Vector3 postition, Vector2Int gridPosition)
    {
        _currentGlobalPosition = postition;
        GridPosition = gridPosition;

        transform.position = _currentGlobalPosition;
    }

    private void SetPlayerColor()
    {
        _mainColor.color = GameSettings.PlayerColor;
        _isPlayer = true;
    }

    private void SetEnemyColor()
    {
        _mainColor.color = GameSettings.EnemyColor;
        _isPlayer = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isPlayer == false)
            return;

        _canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isPlayer == false)
            return;

        _canvasGroup.blocksRaycasts = true;

        transform.position = _currentGlobalPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isPlayer == false)
            return;

        _rectTransform.anchoredPosition +=
            eventData.delta / _freeParent.localScale.x;
    }
}
