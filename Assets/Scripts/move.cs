using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class move : MonoBehaviour
{   
    enum joint
    {
        knee,
        elbow,
    };
    [SerializeField]
    joint js= new joint();
    float gyrox;
    float gyroy;
    float accx;
    float accy;
    float accz;
    public string[] data;
    float errgx;

    [SerializeField]
    TMP_Text angText;
    float errgy;

    float errax;

    float erray;

    float erraz;

    int sample_size = 500;

    int scale = 10;

    float alpha = 0.91f;
    public GameObject leg;
    void calibration()
    {
        float sumgx = 1;
        float sumgy = 1;
        float sumax = 1;
        float sumay = 1;
        float sumaz = 1;
        int i = 0;
        while (i < sample_size)
        {   
            gyrox = float.Parse(data[0]);
            gyroy = float.Parse(data[1]);
            accx = float.Parse(data[2]);
            accy = float.Parse(data[3]);
            sumgx *= gyrox;
            sumgy *= gyroy;
            sumax *= accx;
            sumay *= accy;
            sumaz *= accz;
            i += 1;
        }
        errgx = Mathf.Pow(sumgx, 1.0f/ sample_size);
        errgy = Mathf.Pow(sumgy, 1.0f/ sample_size);
        errax = Mathf.Pow(sumax, 1.0f/ sample_size);
        erray = Mathf.Pow(sumay, 1.0f/ sample_size);
        erraz = Mathf.Pow(sumaz, 1.0f/ sample_size);
    }


    // Update is called once per frame
    void FixedUpdate()
    {   
        if(data.Length>3)
        {
        if (Input.GetKeyDown(KeyCode.C))
        {
            calibration();
        }       
            gyrox = float.Parse(data[0]) + (-1) * errgx;
            gyroy = float.Parse(data[1]) + (-1) * errgy;
            accx = float.Parse(data[2]) + (-1) * errax;
            accy = float.Parse(data[3]) + (-1) * erray;
            Debug.Log(data);
            int angle_x = (int)((alpha * accx + (1 - alpha) * gyrox) * scale);
            int angle_y = (int)((alpha * accy + (1 - alpha) * gyroy) * scale);
            Debug.Log(angle_y);
            switch (js)
            {
                
                case joint.elbow:
                    leg.transform.localEulerAngles =  Vector3.Lerp(new Vector3(leg.transform.localRotation.x,leg.transform.localRotation.y,leg.transform.localRotation.z),(new Vector3(leg.transform.localRotation.x,leg.transform.localRotation.y,(angle_x))),1f);
                    break;
                case joint.knee:
                    leg.transform.localEulerAngles =  Vector3.Lerp(new Vector3(leg.transform.localRotation.x,leg.transform.localRotation.y,leg.transform.localRotation.z),(new Vector3(angle_x,leg.transform.localRotation.y,leg.transform.localRotation.z)),1f);  
                    break;
            }
            
            angText.text = "ANGLE: "+angle_x+"° ";
        }
    }
}
