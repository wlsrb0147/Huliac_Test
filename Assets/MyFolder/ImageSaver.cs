using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ImageSaver : MonoBehaviour
{
    public static ImageSaver instance;
    
    [SerializeField]
    private Sprite[] sprites;
    
    [SerializeField]
    private GameObject popup;
    
    private float _timer = 0f;
    

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("CreatedInstance");
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        _timer += Time.deltaTime;
        if (_timer >= 3.0f && popup.activeSelf)
        {
            popup.SetActive(false);
        }
    }

    public void ChangeSprite(int x)
    {
        popup.SetActive(true);
        popup.GetComponent<Image>().sprite = sprites[x];
        _timer = 0.0f;
        
        // _imageSaver.currentSprite = Resources.Load<Sprite>("Button" + buttonNumber);
    }
}
