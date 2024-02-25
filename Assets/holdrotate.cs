using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class holdrotate : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public Rotation hd;

    public void OnPointerDown(PointerEventData eventData) {
        hd.isPressing=true;
        Debug.Log("start hold");
    }

    public void OnPointerUp(PointerEventData eventData) {
        hd.isPressing=false;
        Debug.Log("stop hold");
    }
}
