using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Text text;
    [SerializeField] SettingUI settingUiPrefab;

    public void Click()
    {
        Debug.Log("����â");
        Manager.UI.ShowPopUpUI(settingUiPrefab);
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
