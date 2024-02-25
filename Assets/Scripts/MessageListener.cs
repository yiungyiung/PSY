using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageListener : MonoBehaviour
{
    public string[] data;
    public movesoham leg;
    void OnMessageArrived(string msg)
    {   
        string[] data =msg.Split(',');
        if (data.Length>3)
        {
            leg.data = data;
        }

    }


    
    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }

}