using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(VideoPlayer))]
public class VideoOnGUI : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public GameObject mask;

    public void Awake()
    {
        if (rawImage == null)
            rawImage = GetComponent<RawImage>();
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        videoPlayer.Play();
        if (videoPlayer.texture == null)
            return;
        rawImage.texture = videoPlayer.texture;
        rawImage.color = Color.white;
    }

    private void Update()
    {
        if (videoPlayer.texture == null)
            return;
        rawImage.texture = videoPlayer.texture;
        rawImage.color = Color.white;
    }
    //Gamef.DelayedExecution(delegate
    //    {
    //        if (mask.activeSelf)
    //        {
    //            mask.SetActive(false);
    //        }
    //    }, 0.01f);
}
