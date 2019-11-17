using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ControlButton : MonoBehaviour
{
    public VideoClip clip;
    public string fileName;
    public VideoPlayer playerComponent;

    public void setClip()
    {
//#if UNITY_EDITOR
//        playerComponent.source = VideoSource.VideoClip;
//        playerComponent.clip = clip;
//#elif UNITY_WEBGL
        playerComponent.source = VideoSource.Url;
        playerComponent.url = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
//#endif
        playerComponent.Play();
    }
}
