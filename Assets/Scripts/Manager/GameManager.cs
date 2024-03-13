using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // ���� ����Ϸ���
    public PlayerMove player;
    public HpBar hpBar;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        hpBar = FindObjectOfType<HpBar>();
    }
}
