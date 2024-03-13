using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : WindowUI
{
    private void Awake()
    {
        base.Awake();
        GetUI<Button>("InventoryExitButton").onClick.AddListener(Close);
    }
}
