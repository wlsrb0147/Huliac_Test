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
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _imageSaver.isMovieError = false;
        _imageSaver.currentOpen = -1;
    }
}
