using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Text text;
    [SerializeField] SettingUI settingUIPrefab;

    public void Click()
    {
        Debug.Log("����â");
        Manager.UI.ShowPopUpUI(settingUIPrefab);
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
