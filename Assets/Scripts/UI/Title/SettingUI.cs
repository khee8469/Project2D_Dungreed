using UnityEngine.UI;

public class SettingUI : PopUpUI
{
    protected override void Awake()
    {
        //SettingUI의 자식 정보를 가져올수있게 딕셔너리에 저장해두는 작업
        base.Awake();


        GetUI<Button>("SettingExitButton").onClick.AddListener(Close);  // PopUpUI의 Close사용
    }

    //버튼 추가
}
