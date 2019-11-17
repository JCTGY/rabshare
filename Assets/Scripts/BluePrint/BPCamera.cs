using System.IO;
using UnityEngine;
using Unity.Collections;

public class BPCamera : MonoBehaviour
{
    public Color32[] imageArray;
    //public NativeArray<Color32> imageNativeArray;

    private void Start()
    {
        //imageArray = null;
    }
    public void CamCapture()
    {
        Camera Cam = GetComponent<Camera>();

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = Cam.targetTexture;

        Cam.Render();

        Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0, false);
        Image.Apply();
        RenderTexture.active = currentRT;
        imageArray = Image.GetPixels32();
        //imageNativeArray = Image.GetRawTextureData<Color32>();
        Destroy(Image);
    }
}
