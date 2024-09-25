using UnityEngine;
using UnityEngine.UI;

public class ErrorText : MonoBehaviour
{
    public Text errortext;

    public void CloseTab()
    {
        gameObject.SetActive(false);
    }
}
