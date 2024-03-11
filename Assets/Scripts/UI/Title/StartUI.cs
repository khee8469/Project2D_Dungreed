
using UnityEngine.UI;

public class StartUI : PopUpUI
{
    protected override void Awake()
    {
        //SettingUI�� �ڽ� ������ �����ü��ְ� ��ųʸ��� �����صδ� �۾�
        base.Awake();

        //SettingExitButton��ư Ŭ���� Close �Լ� �ߵ��ϰ� ����
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
