using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : PopUpUI
{
    protected override void Awake()
    {
        //SettingUI의 자식 정보를 가져올수있게 딕셔너리에 저장해두는 작업
        base.Awake();

        GetUI<Button>("PauseUIExitButton").onClick.AddListener(Close);  // PopUpUI의 Close사용
    }

}
