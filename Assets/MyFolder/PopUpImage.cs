using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupImage : MonoBehaviour,IPointerDownHandler
{
    private ImageSaver _imageSaver;
    private void Awake()
    {
        if (!_imageSaver)
        {
            _imageSaver = ImageSaver.instance;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _imageSaver.timer = 0;
        _imageSaver.Close(_imageSaver.currentOpen);
    }

    private void OnEnable()
    {
        _imageSaver.timer = 0;
    }

    private void OnDisable()
    {
        _imageSaver.currentOpen = -1;
        _imageSaver.timer = 0;
    }
}
