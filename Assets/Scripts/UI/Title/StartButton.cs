using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Text text;
    [SerializeField] StartUI startUIPrefab;

    public void Click()
    {
        if(startUIPrefab == null)
        {
            Debug.Log(startUIPrefab);
            return;
        }
        Manager.UI.ShowPopUpUI(startUIPrefab);
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
