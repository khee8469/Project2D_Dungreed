using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // ���� ����Ϸ���
    public PlayerMove player;
    public HpBar hpBar;


    public ItemDatabase itemDatabase;
    public InventoryUI inventoryUI;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        hpBar = FindObjectOfType<HpBar>();


        itemDatabase = FindObjectOfType<ItemDatabase>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        
    }
}
