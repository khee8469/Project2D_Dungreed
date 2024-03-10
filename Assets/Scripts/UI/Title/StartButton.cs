using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Text text;

    public void Click()
    {
        Debug.Log("로드UI창");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = Color.white;
    }

    // 마우스가 UI 요소에서 벗어날 때 호출됨
    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.gray;
    }
}
