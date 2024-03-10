using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SteamButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image steam;

    public void Click()
    {
        Debug.Log("Ω∫∆¿¿Ãµø");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        steam.color = new Color(0, 0, 0, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        steam.color = new Color(255, 255, 255, 255);
    }
}
