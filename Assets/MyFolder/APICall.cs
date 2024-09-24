using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class APICall : MonoBehaviour
{
    [SerializeField] private GameObject backGround;
    
    void Start()
    {
        string url = "http://huliac.com/random.cfm?name=test";
        StartCoroutine(GetDataFromApi(url));
    }

    IEnumerator GetDataFromApi(string url)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();
            
        if (webRequest.result == UnityWebRequest.Result.ConnectionError 
            || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogWarning("Error :" + webRequest.error);
        }
        else
        {
            string jsonResponse = webRequest.downloadHandler.text;
            Debug.Log("Response : " + jsonResponse);

            string[] colorArray = jsonResponse.Split(',');
                
            int[] intArray = new int[3];
                
            intArray[0] = Convert.ToInt32(colorArray[0]);
            intArray[1] = Convert.ToInt32(colorArray[1]);
            intArray[2] = Convert.ToInt32(colorArray[2]);

            float x = (float)intArray[0]/255;
            float y = (float)intArray[1]/255;
            float z = (float)intArray[2]/255;
                
                
            backGround.GetComponent<Image>().color = new Color(a:1, r: x, g: y, b: z);
            Debug.Log(backGround.GetComponent<Image>().color);
            Debug.Log(Color.magenta);
        }
    }
}
