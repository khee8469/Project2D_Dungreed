using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = GetComponent<InventoryUI>();
    }
}
