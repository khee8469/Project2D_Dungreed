using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemDestoryUI : WindowUI
{
    protected override void Awake()
    {
        //SettingUI�� �ڽ� ������ �����ü��ְ� ��ųʸ��� �����صδ� �۾�
        base.Awake();
        //SettingExitButton��ư Ŭ���� Close �Լ� �ߵ��ϰ� ����
        GetUI<Button>("YesButton").onClick.AddListener(ItemDestory);
        GetUI<Button>("NoButton").onClick.AddListener(Close);
    }
    private void ItemDestory()
    {
        DragSlot.instance.dragSlot.ClearSlot();
        Close();
    }
}
