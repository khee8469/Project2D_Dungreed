using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : PopUpUI
{
    protected override void Awake()
    {
        //SettingUI�� �ڽ� ������ �����ü��ְ� ��ųʸ��� �����صδ� �۾�
        base.Awake();

        //SettingExitButton��ư Ŭ���� Close �Լ� �ߵ��ϰ� ����
        GetUI<Button>("SettingExitButton").onClick.AddListener(Close);  // PopUpUI�� Close���
    }

    
    //��ư �߰�
}
