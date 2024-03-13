using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUI : WindowUI
{
    private void Awake()
    {
        base.Awake();
        GetUI<Button>("StateExitButton").onClick.AddListener(Close);
    }
}