using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : PopUpUI
{
    protected override void Awake()
    {
        //SettingUI�� �ڽ� ������ �����ü��ְ� ��ųʸ��� �����صδ� �۾�
        base.Awake();

        GetUI<Button>("PauseUIExitButton").onClick.AddListener(Close);  // PopUpUI�� Close���
    }

}
