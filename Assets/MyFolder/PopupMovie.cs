using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class PopupMovie : MonoBehaviour, IPointerClickHandler
{
    public string[] MoviePath { get; set; }
    [SerializeField] private VideoPlayer videoPlayer;
    private RectTransform _rectTransform;
    private RawImage _rawImage;
    [SerializeField] private RenderTexture renderTexture;
    private ImageSaver _imageSaver;

    private const float MaxWidth = 1920f;
    private const float MaxHeight = 1080f;

    private float _timer;
    private int _currentPlay;
    
    private void Awake()
    {
        MoviePath = new string[5];
        _rawImage = GetComponent<RawImage>();
        _rectTransform = GetComponent<RectTransform>();
        
        if (!_imageSaver)
        {
            _imageSaver = ImageSaver.instance;
        }
    }

    private void Start()
    {
        _rawImage.texture = renderTexture;
        // 비디오 소소는 Url로 가져옴
        videoPlayer.source = VideoSource.Url;
        // 동영상이 루프포인트 도달할떄마다 OnvideoEnd(~~) 실행
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void FixedUpdate()
    {
        if (!videoPlayer.isPlaying)
        {
            _timer += Time.fixedDeltaTime;

            if (_timer > 10.0f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void PlayMovie(int x)
    {
        if (renderTexture)
        {
            renderTexture.Release();
        }

        _currentPlay = x;
        // 비디오가 재생 준비 될때마다 AdjustSize(~~) 실행
        videoPlayer.prepareCompleted += AdjustSize;
        // 비디오 경로 설정
        
        videoPlayer.url = MoviePath[x];
        videoPlayer.Prepare();
        
    }

    
    private void AdjustSize(VideoPlayer source)
    {
        if (renderTexture)
        {
            renderTexture.Release();
        }
        
        float videoWidth = videoPlayer.texture.width;
        float videoHeight = videoPlayer.texture.height;
        
        Vector2 adjustedSize = GetAdjustedSize(videoWidth, videoHeight);
        _rectTransform.sizeDelta = adjustedSize;
        
        renderTexture.width = Convert.ToInt32(adjustedSize.x);
        renderTexture.height = Convert.ToInt32(adjustedSize.y);
        renderTexture.Create();
        videoPlayer.Play();
    }

    private Vector2 GetAdjustedSize(float videoWidth, float videoHeight)
    {
        float ratio = videoHeight / videoWidth;
        float targetratio = MaxHeight / MaxWidth;

        float targetWidth;
        float targetHeight;

        // 높이 비율이 더 높음
        if (ratio >= targetratio)
        {
            targetHeight = 1080f;
            targetWidth = 1080f / ratio;
        }
        else
        {
            targetWidth = 1920f;
            targetHeight = 1920f * ratio;
        }
        
        return new Vector2(targetWidth, targetHeight);
    }

    private void OnVideoEnd(VideoPlayer source)
    {
        //VideoOff();
    }

    public void VideoOff()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _timer = 0.0f;
        _imageSaver.currentOpen = -1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            _timer = 0.0f;
            videoPlayer.Play();
        }
    }
}
