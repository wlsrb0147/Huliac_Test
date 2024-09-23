using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopUp : MonoBehaviour,IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }
}
