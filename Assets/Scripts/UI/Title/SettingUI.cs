using UnityEngine.UI;

public class SettingUI : PopUpUI
{
    protected override void Awake()
    {
        //SettingUI�� �ڽ� ������ �����ü��ְ� ��ųʸ��� �����صδ� �۾�
        base.Awake();


        GetUI<Button>("SettingExitButton").onClick.AddListener(Close);  // PopUpUI�� Close���
    }

    //��ư �߰�
}
