using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageSaver : MonoBehaviour
{
    public static ImageSaver instance;
    SoundManager _soundManager;
    
    private Settings _settings;
    
    [SerializeField] private Button[] buttons; // 버튼 다섯개 설정 완료
    private string[] _btnImagePath; // 이미지 패스 가져옴
    private readonly Sprite[] _btnSprites = new Sprite[5]; // 시작시 세팅에서 다섯개 이미지 가져옴
    
    [SerializeField] private GameObject popup;
    private readonly Sprite[] _popupSprite = new Sprite[5];
    [SerializeField] private Sprite[] popupTempSprite;

    private float _timer;
    private float _timeLimit;
    
    private string[] _keyCodes;
    private readonly KeyCode[] _loadedKeys = new KeyCode[5];
    
    private Vector2[] _location;
    private Vector2[] _size;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Cursor.visible = false;
    }

    void Start()
    {
        if (_soundManager == null)
        {
            _soundManager = SoundManager.instance;
        }
        LoadSettings(LoadData);
    }

    // Update is called once per frame

    private void Update()
    {
        for(int i=0; i<buttons.Length; i++)
        {
            if (Input.GetKeyDown(_loadedKeys[i]))
            {
                if (popup.activeSelf)
                {
                    popup.SetActive(false);
                }
                else
                {
                    ChangeSprite(i);
                }
            }
        }
    }
    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        if (_timer >= _timeLimit && popup.activeSelf)
        {
            popup.SetActive(false);
        }
    }

    public void ChangeSprite(int x)
    {
        popup.SetActive(true);
        popup.GetComponent<Image>().sprite = _popupSprite[x];
        _timer = 0.0f;
        _soundManager.PlaySound();
        // _imageSaver.currentSprite = Resources.Load<Sprite>("Button" + buttonNumber);
    }

    private void LoadSettings(Action onComplete)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Settings.json");
        filePath = filePath.Replace("/", "\\");
        
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            _settings = JsonUtility.FromJson<Settings>(jsonString);
            
            Debug.Log(jsonString);
            
            if (_settings.ImagePaths != null)
            {
                Debug.Log("length of Path : " + _settings.ImagePaths.Length);
                foreach (string path in _settings.ImagePaths)
                {
                    Debug.Log("imagePaths item : " + path);
                }
            }
            else
            {
                Debug.LogWarning("imagePaths is null");
            }

            if (_settings.KeyCodes != null)
            {
                Debug.Log("length of KeyCode : " + _settings.KeyCodes.Length);
                foreach (var code in _settings.KeyCodes)
                {
                    Debug.Log("keycode item : " + code);
                }
            }
            else
            {
                Debug.LogWarning("keycode is null");
            }
        }
        else
        {
            Debug.LogWarning("Failed to load Settings...");
        }
        
        _btnImagePath = _settings.ImagePaths;
        
        onComplete?.Invoke();
    }

    void LoadData()
    {
        Debug.Log("Loading Image...");
        
        // 이미지 로딩
        for (int i = 0; i < _btnImagePath.Length; i++)
        {    
            byte[] fileData = File.ReadAllBytes(_btnImagePath[i]);
            char extractedNumber = _btnImagePath[i][_btnImagePath[i].Length - 5];
            int number = Convert.ToInt16(extractedNumber) - '0';
            --number;
            
            Debug.Log("추출한 경로 : " + _btnImagePath[i]);
            Debug.Log("추출한 숫자 : " + number);
            _popupSprite[i] = popupTempSprite[number];
            
            Debug.Log(popupTempSprite[i].name);
            Debug.Log(_popupSprite[i].name);
            
            Texture2D texture = new Texture2D(2, 2);
            
            texture.LoadImage(fileData);

            if (texture != null)
            {
                Debug.Log("i : " + i);
                _btnSprites[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                buttons[i].GetComponent<Image>().sprite = _btnSprites[i];
            }
        }
        Debug.Log("이미지 로드 종료");

        // 타이머
        _timeLimit = _settings.Timer;
        
        // 키바인딩
        _keyCodes = _settings.KeyCodes;
        
        Debug.Log(_keyCodes.Length);

        for (int i = 0; i < _keyCodes.Length; i++)
        {
            _loadedKeys[i] = (KeyCode)Enum.Parse(typeof(KeyCode), _keyCodes[i]);
        }
        
        Debug.Log("키바인딩 종료");
        
        // 좌표, 사이즈
        _location = _settings.Locations;
        _size = _settings.Size;
        Debug.Log(_location.Length);
        
        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform rectTransform = buttons[i].GetComponent<RectTransform>();

            if (!buttons[i])
            {
                Debug.LogWarning("button is empty");
            }
            
            rectTransform.anchoredPosition = _location[i];
            rectTransform.sizeDelta = _size[i];
        }
    }
}
