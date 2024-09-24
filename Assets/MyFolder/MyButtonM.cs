using UnityEngine;

public class MyButtonM : MonoBehaviour
{
    [SerializeField]
    private int buttonNumber = 0;
    ImageSaver _imageSaver;
    SoundManager _soundManager;
    private void Start()
    {
        if (!_imageSaver)
        {
            _imageSaver = ImageSaver.instance;
        }

        if (!_soundManager)
        {
            _soundManager = SoundManager.instance;
        }
    }

    public void ButtonDown()
    {
        _imageSaver.ChangeSprite(buttonNumber);
    }
}
