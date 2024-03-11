
using UnityEngine.UI;

public class StartUI : PopUpUI
{
    protected override void Awake()
    {
        //SettingUI의 자식 정보를 가져올수있게 딕셔너리에 저장해두는 작업
        base.Awake();

        //SettingExitButton버튼 클릭시 Close 함수 발동하게 설정
        GetUI<Button>("Load1Button").onClick.AddListener(GameLoad);
        GetUI<Button>("Load2Button").onClick.AddListener(GameLoad);
        GetUI<Button>("Load3Button").onClick.AddListener(GameLoad);
        GetUI<Button>("StartExitButton").onClick.AddListener(Close);
    }

    public void GameLoad()
    {
        Manager.Scene.LoadScene("MainGame");
    }
}
