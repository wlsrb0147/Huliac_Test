using System;
using UnityEngine;

public class MyButtonM : MonoBehaviour
{
    [SerializeField]
    private int buttonNumber = 0;
    ImageSaver _imageSaver;
    private void Start()
    {
        if (!_imageSaver)
        {
            _imageSaver = ImageSaver.instance;
        }
    }

    public void ButtonDown()
    {
        _imageSaver.ChangeSprite(buttonNumber);
    }
}
