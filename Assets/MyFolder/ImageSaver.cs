using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ImageSaver : MonoBehaviour
{
    // 싱글톤
    public static ImageSaver instance;
    SoundManager _soundManager;
    
    // 제이슨
    private Settings _settings;
    
    /// 버튼
    [SerializeField] private Button[] buttons; // 버튼 다섯개 설정 완료
    private string[] _btnImagePath; // 이미지 패스 가져옴
    private readonly Sprite[] _btnSprites = new Sprite[5]; // 시작시 세팅에서 다섯개 이미지 가져옴
    
    // 팝업 제어
    private bool[] _seeMovie;
    
    // 팝업 이미지
    [SerializeField] private GameObject popupImage;
    private readonly Sprite[] _popupSprite = new Sprite[5];
    [SerializeField] private Sprite[] popupTempSprite;
    
    // 팝업 movie
    [SerializeField] private GameObject popupMovie;
    private string[] _moviePath;
    private PopupMovie _pm;

    // 타이머
    [HideInInspector] public float timer;
    private float _timeLimit;
    
    // 키 바인딩
    private string[] _keyCodes;
    private readonly KeyCode[] _loadedKeys = new KeyCode[5];
    
    // 버튼 크기, 사이즈
    private Vector2[] _location;
    private Vector2[] _size;
    
    /// Pair 사용
    private string[] _secondPopupPath;
    private readonly Sprite[] _secondPopup = new Sprite[5];
    private Vector2[] _pairBtn;
    private Vector2[] _pairImg;
    private Vector2[] _pairMov;
    private readonly Dictionary<int,int> _dicBtn = new();
    private readonly Dictionary<int,int> _dicImg = new();
    private readonly Dictionary<int,int> _dicMov = new();

    /// Toggle
    [SerializeField] private Toggle mouseVisibleToggle;
    [SerializeField] private Toggle jsonImageControl;
    [SerializeField] private Toggle jsonMovieControl;
    [SerializeField] private Toggle[] movieToggle;
    
    // 팝업 제어
    [HideInInspector] public int currentOpen = -1;
    
    private int _beforeDic;
    
    private void AddDictionary()
    {
        _dicBtn.Add((int)_pairBtn[0].x-1,(int)_pairBtn[0].y-1);
        _dicBtn.Add((int)_pairBtn[1].x-1,(int)_pairBtn[1].y-1);
        _dicBtn.Add((int)_pairBtn[2].x-1,(int)_pairBtn[2].y-1);
        _dicBtn.Add((int)_pairBtn[3].x-1,(int)_pairBtn[3].y-1);
        _dicBtn.Add((int)_pairBtn[4].x-1,(int)_pairBtn[4].y-1);
        
        _dicImg.Add((int)_pairImg[0].x-1,(int)_pairImg[0].y-1);
        _dicImg.Add((int)_pairImg[1].x-1,(int)_pairImg[1].y-1);
        _dicImg.Add((int)_pairImg[2].x-1,(int)_pairImg[2].y-1);
        _dicImg.Add((int)_pairImg[3].x-1,(int)_pairImg[3].y-1);
        _dicImg.Add((int)_pairImg[4].x-1,(int)_pairImg[4].y-1);
        
        _dicMov.Add((int)_pairMov[0].x-1,(int)_pairMov[0].y-1);
        _dicMov.Add((int)_pairMov[1].x-1,(int)_pairMov[1].y-1);
        _dicMov.Add((int)_pairMov[2].x-1,(int)_pairMov[2].y-1);
        _dicMov.Add((int)_pairMov[3].x-1,(int)_pairMov[3].y-1);
        _dicMov.Add((int)_pairMov[4].x-1,(int)_pairMov[4].y-1);
    }
    
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
    }

    void Start()
    {
        if (_soundManager == null)
        {
            _soundManager = SoundManager.instance;
        }
        LoadSettings(LoadData);
        
        
        
        // 딕셔너리. map과 같은 기능
    }
    
    private void Update()
    {
        for(int i=0; i<buttons.Length; i++)
        {
            if (Input.GetKeyDown(_loadedKeys[i]))
            {
                OpenPopup(i);
            }
        }
        
        if (mouseVisibleToggle.isOn)
        {
            if (!Cursor.visible)
            {
                Cursor.visible = true;
            }
        }
        else
        {
            if (Cursor.visible)
            {
                Cursor.visible = false;
            }
        }
    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= _timeLimit && popupImage.activeSelf)
        {
            popupImage.SetActive(false);
        }
    }

    public void OpenPopup(int x) // 값 : 0~4
    {
        if (currentOpen == x)
        {
            popupImage.SetActive(false);
            popupMovie.SetActive(false);
            
            return;
        }
        
        bool isMovie;
        
        if (jsonMovieControl.isOn)
        {
            isMovie = _seeMovie[x];
        }
        else
        {
            isMovie = movieToggle[x].isOn;
        }

        bool isExistMovie = File.Exists(_moviePath[_dicMov[x]]);
        bool isExistFirstPop = _popupSprite[x];
        bool isExistSecondPop = _secondPopup[_dicImg[x]];
        
        if (jsonMovieControl.isOn)
        {
            if (!isExistMovie && isExistSecondPop) // 이미지와 영화 둘 다 없는경우
            {
                Debug.LogWarning(x+1 +"번 버튼의 이미지와 영상 둘 다 부재");
                currentOpen = -1;
                timer = 0;
                return;
            }
            
            if (isMovie) // 영화 보고싶음
            {
                if (isExistMovie) // 영화 있음
                {
                    // 영화 재생
                    popupImage.SetActive(false);
                    popupMovie.SetActive(true);
                    _pm.PlayMovie(_dicMov[x]);
                }
                else // 영화 없음
                {
                    // 이미지 재생
                    popupMovie.SetActive(false);
                    popupImage.SetActive(true);
                    popupImage.GetComponent<Image>().sprite = _secondPopup[_dicImg[x]];
                    Debug.Log("이미지 재생");
                }
            }
            else // 영화 안보고싶음
            {
                if (isExistSecondPop) // 이미지 있음
                {
                    // 이미지 재생
                    popupMovie.SetActive(false);
                    popupImage.SetActive(true);
                    popupImage.GetComponent<Image>().sprite = _secondPopup[_dicImg[x]];
                }
                else // 이미지 없음
                {
                    // 영화 재생
                    popupImage.SetActive(false);
                    popupMovie.SetActive(true);
                    _pm.PlayMovie(_dicMov[x]);
                }
            }
        }
        else
        {
            if (!isExistMovie && isExistFirstPop) // 이미지와 영화 둘 다 없는경우
            {
                Debug.LogWarning(x+1 +"번 버튼의 이미지와 영상 둘 다 부재");
                currentOpen = -1;
                timer = 0;
                return;
            }
            
            if (isMovie) 
            {
                if (isExistMovie)
                {
                    // 영화 재생
                    popupImage.SetActive(false);
                    popupMovie.SetActive(true);
                    _pm.PlayMovie(_dicMov[x]);
                }
                else
                {
                    // 이미지 재생
                    popupMovie.SetActive(false);
                    popupImage.SetActive(true);
                    popupImage.GetComponent<Image>().sprite = _popupSprite[x];
                }
            }
            else // 영화 안보고싶음
            {
                if (isExistFirstPop)
                {
                    // 이미지 재생
                    popupMovie.SetActive(false);
                    popupImage.SetActive(true);
                    popupImage.GetComponent<Image>().sprite = _popupSprite[x];
                }
                else
                {
                    // 영화 재생
                    popupImage.SetActive(false);
                    popupMovie.SetActive(true);
                    _pm.PlayMovie(_dicMov[x]);
                }
            }
        }
        currentOpen = x;
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
        }
        else
        {
            Debug.LogWarning("Failed to load Settings...");
        }
        
        _btnImagePath = _settings.ImagePaths;
        Debug.Log("버튼 길이 : " + _btnImagePath.Length);
        _secondPopupPath = _settings.PopupPaths;
        Debug.Log("팝업 길이 : " + _secondPopupPath.Length);
        _moviePath = _settings.MoviePaths;
        Debug.Log("영화 길이 : " + _moviePath.Length);
        _seeMovie = _settings.SeeMovie;
        Debug.Log("씨 무비 길이 : " + _seeMovie.Length);
        _timeLimit = _settings.Timer;
        Debug.Log("시간 : " + _timeLimit);
        _keyCodes = _settings.KeyCodes;
        Debug.Log("키코드 길이 : " + _keyCodes.Length);
        _location = _settings.Locations;
        Debug.Log("위치 길이 : " + _location.Length);
        _size = _settings.Size;
        Debug.Log("크기 길이 : " + _size.Length);
        _pairBtn = _settings.ButtonPair;
        Debug.Log("버튼페어 길이 : " + _pairBtn.Length);
        _pairImg = _settings.PopupPair;
        Debug.Log("팝업페어 길이 : " + _pairImg.Length);
        _pairMov = _settings.MoviePair;
        Debug.Log("무비페어 길이 : " + _pairMov.Length);

        AddDictionary();
        
        onComplete?.Invoke();
    }

    void LoadData()
    {
        Debug.Log("Loading Data...");
     
        const int lengthOfButtons = 5;

        // 버튼 이미지 저장
        for (int i = 0; i < lengthOfButtons; i++)
        {
            try
            {
                byte[] fileData = File.ReadAllBytes(_btnImagePath[i]); // 파일을 byte로 받아옴
                Texture2D texture = new Texture2D(2, 2); 
                texture.LoadImage(fileData); 
                _btnSprites[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
           
        }

        // 버튼 이미지 할당
        for (int i = 0; i < lengthOfButtons; i++)
        {
            buttons[i].GetComponent<Image>().sprite = _btnSprites[_dicBtn[i]];
        }
        
        // 패스 이름 기반 이미지 할당
        for (int i = 0; i < lengthOfButtons; i++)
        {    
            char extractedNumber = _btnImagePath[i][_btnImagePath[i].Length - 5];
            int number = Convert.ToInt16(extractedNumber) - '0';
            --number;
            
            _popupSprite[i] = popupTempSprite[number];
        }
        Debug.Log("이미지 로드 종료");
        
        // second popup
        for (int i = 0; i < lengthOfButtons; i++)
        {
            try
            {
                byte[] fileData = File.ReadAllBytes(_secondPopupPath[i]); 
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);
                _secondPopup[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        for (int i = 0; i < lengthOfButtons; i++)
        {
            _loadedKeys[i] = (KeyCode)Enum.Parse(typeof(KeyCode), _keyCodes[i]);
        }
        
        Debug.Log("키바인딩 종료");
        
        // 위치, 크기
        for (int i = 0; i < lengthOfButtons; i++)
        {
            RectTransform rectTransform = buttons[i].GetComponent<RectTransform>();

            if (!buttons[i])
            {
                Debug.LogWarning("button is empty");
            }
            
            rectTransform.anchoredPosition = _location[i];
            rectTransform.sizeDelta = _size[i];
        }
        
        // 영화 팝업에 경로 저장
        popupMovie.SetActive(true);
        _pm = popupMovie.GetComponent<PopupMovie>();
        for (int i = 0; i < 5; i++)
        {
            _pm.MoviePath[i] = _moviePath[i];
        }
        popupMovie.SetActive(false);
    }
}
