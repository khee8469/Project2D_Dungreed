
using UnityEngine.UI;

public class StartUI : PopUpUI
{
    protected override void Awake()
    {
        //SettingUI의 자식 정보를 가져올수있게 딕셔너리에 저장해두는 작업
        base.Awake();

        //SettingExitButton버튼 클릭시 Close 함수 발동하게 설정
        GetUI<Button>("Load1Button").onClick.AddListener(() => GameLoad(0));
        GetUI<Button>("Load2Button").onClick.AddListener(() => GameLoad(1));
        GetUI<Button>("Load3Button").onClick.AddListener(() => GameLoad(2));
        GetUI<Button>("StartExitButton").onClick.AddListener(Close);
    }

    public void GameLoad(int index)
    {
        if (Manager.Data.ExistData(index))
        {
            Manager.Data.LoadData(index);
            Manager.Scene.LoadScene("Town");
        }
        else if (Manager.Data.GameData != null)
        {
            Manager.Data.NewData();
            Manager.Scene.LoadScene("Town");
        }
    }
}
