using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using static UnityEditor.Progress;

public class ItemDestoryUI : WindowUI
{
    protected override void Awake()
    {
        //SettingUI의 자식 정보를 가져올수있게 딕셔너리에 저장해두는 작업
        base.Awake();
        //SettingExitButton버튼 클릭시 Close 함수 발동하게 설정
        GetUI<Button>("YesButton").onClick.AddListener(ItemDestory);
        GetUI<Button>("NoButton").onClick.AddListener(Close);
    }
    private void ItemDestory()
    {
        DragSlot.instance.dragSlot.ClearSlot();
        Close();
    }

    private void CloseUI()
    {
        Manager.UI.CloseWindowUI(this);
        DragSlot.instance.dragSlot.SetColor(1);
    }
}
