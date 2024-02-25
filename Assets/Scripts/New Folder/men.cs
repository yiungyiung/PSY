using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class men : MonoBehaviour
{
    public void changescene()
    {
        if(SceneManager.GetActiveScene().name=="knee")
        {
            SceneManager.LoadScene("elbow");
        }
        else
        {
            SceneManager.LoadScene("knee");
        }
    }
}
