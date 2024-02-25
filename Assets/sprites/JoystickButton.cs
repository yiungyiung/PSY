using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform pivot; // Pivot RectTransform for the joystick's center
    public float maxRadius = 50f; // Maximum radius for the joystick movement

    private bool isDragging = false;
    private Vector2 startDragPosition;
    private Vector2 currentDragPosition;

    private void Start()
    {
        //pivot.localPosition = Vector3.zero; // Initialize pivot's position to the center
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        startDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 dragOffset = eventData.position - startDragPosition;
            currentDragPosition = Vector2.ClampMagnitude(dragOffset, maxRadius);

            pivot.localPosition = currentDragPosition;

            // Add logic here to check for interactions with other elements
            // You can use Physics.Raycast, Physics2D.Raycast, or other methods to detect overlaps.
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        pivot.localPosition = Vector3.zero; // Reset pivot's position when releasing
    }
}