using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Transform pivot; // The central pivot point
    public float maxRadius = 100f; // The maximum allowed radius

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pivotToCursor = eventData.position - (Vector2)pivot.position;
        Vector2 clampedPosition = Vector2.ClampMagnitude(pivotToCursor, maxRadius);

        rectTransform.anchoredPosition = clampedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Perform any logic you need when dragging ends
    }
}
