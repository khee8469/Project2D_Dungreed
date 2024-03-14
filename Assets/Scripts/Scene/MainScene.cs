using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainScene : BaseScene
{
    [SerializeField] PauseUI pauseUIPrefab;
    [SerializeField] InventoryUI InventoryUIPrefab;
    [SerializeField] StateUI stateUIPrefab;
    [SerializeField] WindowUI inventory;
    [SerializeField] WindowUI state;
    //public bool isPause { get; private set; }
    private void OnPause(InputValue value)
    {
        if(Manager.UI.PopUpStack.Count == 0)
        {
            Manager.UI.ShowPopUpUI(pauseUIPrefab);
        }
        else
        {
            Manager.UI.ClosePopUpUI();
        }

        
        //isPause = !isPause; // true를 false로 false를 true로
    }
    /*private void OnDisPause()
    {

        isPause = !isPause;
    }*/

    private void OnInventoryOpen(InputValue value) 
    {
        if (!FindAnyObjectByType<InventoryUI>())
        {
            inventory = Manager.UI.ShowWindowUI(InventoryUIPrefab);
        }
        else
        {
            Manager.UI.CloseWindowUI(inventory);
        }
    }

    private void OnStateOpen(InputValue value)
    {
        if (!FindAnyObjectByType<StateUI>())
        {
            state = Manager.UI.ShowWindowUI(stateUIPrefab);
        }
        else
        {
            Manager.UI.CloseWindowUI(state);
        }
    }



    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
