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
        Debug.Log("설정창");
        Manager.UI.ShowPopUpUI(settingUiPrefab);
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
