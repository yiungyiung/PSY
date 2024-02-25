using UnityEngine;
using UnityEngine.EventSystems;

public class test01 : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Transform pivot; // The central pivot point
    public float maxRadius = 100f; // The maximum allowed radius
    public float returnSpeed = 5f; // Speed at which the button returns to its original position
    public float smoothTime = 0.3F; // Smoothing time for the dragging movement

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Vector2 velocity = Vector2.zero; // This value is modified by the SmoothDamp function each time you call it.

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pivotToCursor = eventData.position - (Vector2)pivot.position;
        Vector2 clampedPosition = Vector2.SmoothDamp(rectTransform.anchoredPosition, pivotToCursor, ref velocity, smoothTime);

        if (clampedPosition.magnitude > maxRadius)
        {
            clampedPosition = clampedPosition.normalized * maxRadius;
        }

        rectTransform.anchoredPosition = clampedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(ReturnToOriginalPosition());
    }

    private System.Collections.IEnumerator ReturnToOriginalPosition()
    {
        Vector2 currentPosition = rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * returnSpeed;
            rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, originalPosition, Mathf.SmoothStep(0f, 1f, elapsedTime));
            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
}
}