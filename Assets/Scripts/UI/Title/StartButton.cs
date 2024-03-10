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
        Debug.Log("�ε�UIâ");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = Color.white;
    }

    // ���콺�� UI ��ҿ��� ��� �� ȣ���
    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.gray;
    }
}
