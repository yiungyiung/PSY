using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float longPressDuration = 1.0f;
    private bool isPressed = false;
    private float pressStartTime = 0.0f;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        pressStartTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        if (Time.time - pressStartTime < longPressDuration)
        {

            Debug.Log("Short Press Detected");
        }
        else
        {
          Debug.Log("Long Press Detected");
        }
    }

    private void Update()
    {
        if (isPressed && Time.time - pressStartTime >= longPressDuration)
        {
 
            Debug.Log("Long Press Detected");
            isPressed = false; // Prevent multiple long press triggers
        }
    }
}