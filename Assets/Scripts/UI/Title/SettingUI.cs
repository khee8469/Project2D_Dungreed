using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : PopUpUI
{
    protected override void Awake()
    {
        //SettingUI의 자식 정보를 가져올수있게 딕셔너리에 저장해두는 작업
        base.Awake();

        //SettingExitButton버튼 클릭시 Close 함수 발동하게 설정
        GetUI<Button>("SettingExitButton").onClick.AddListener(Close);  // PopUpUI의 Close사용
    }

    
    //버튼 추가
}
