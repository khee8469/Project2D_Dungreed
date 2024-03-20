
using UnityEngine.UI;

public class StartUI : PopUpUI
{
    protected override void Awake()
    {
        //SettingUI�� �ڽ� ������ �����ü��ְ� ��ųʸ��� �����صδ� �۾�
        base.Awake();

        //SettingExitButton��ư Ŭ���� Close �Լ� �ߵ��ϰ� ����
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
