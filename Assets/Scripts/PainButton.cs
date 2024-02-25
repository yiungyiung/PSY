using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PainButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{   
    public painarray pain;
    public movesoham legang;
    private bool isBeingHeld = false;
    private float holdTime = 1.0f; // Adjust this time threshold as needed
    private float touchStartTime;
    float startangle;
    [SerializeField]
    string painLevel;

    public void OnPointerDown(PointerEventData eventData)
    {
        isBeingHeld = true;
        touchStartTime = Time.time;
        //startangle=2*(Mathf.Rad2Deg*legang.transform.localRotation.x);
        startangle = legang.gg;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isBeingHeld = false;

        if (Time.time - touchStartTime < holdTime)
        {
            Debug.Log("Touched/Clicked");
            pain.singledata(startangle,painLevel);
        }
        else
        {   
            float lastang=((legang.gg));
            Debug.Log("Hold");
            pain.rangedata(startangle,lastang,painLevel);
        }
    }
}
