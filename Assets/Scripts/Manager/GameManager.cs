using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 서로 사용하려고
    public PlayerMove player;
    public HpBar hpBar;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        hpBar = FindObjectOfType<HpBar>();
    }
}
