using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TextUI : PopUpUI, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Manager.UI.ClosePopUpUI();
    }
}
