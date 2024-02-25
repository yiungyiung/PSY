using UnityEngine;

public class CaptureScreenshot : MonoBehaviour
{
    public Camera myCamera;
    public int width = 1024;
    public int height = 768;

    void Start()
    {
        if (myCamera.targetTexture == null)
        {
            myCamera.targetTexture = new RenderTexture(width, height, 24);
        }
        else
        {
            width = myCamera.targetTexture.width;
            height = myCamera.targetTexture.height;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = myCamera.targetTexture;
            myCamera.Render();
            Texture2D image = new Texture2D(width, height);
            image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            image.Apply();
            RenderTexture.active = currentRT;

            byte[] bytes = image.EncodeToPNG();
            Destroy(image);

            System.IO.File.WriteAllBytes(Application.dataPath + "/Screenshot.png", bytes);
            Debug.Log(Application.dataPath + "/Screenshot.png");
        }
    }
}
