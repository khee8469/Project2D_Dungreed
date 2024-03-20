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

    // 마우스가 UI 요소에서 벗어날 때 호출됨
    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.gray;
    }
}
