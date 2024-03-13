using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainScene : BaseScene
{
    [SerializeField] PauseUI pauseUIPrefab;
    [SerializeField] InventoryUI InventoryUIPrefab;
    [SerializeField] StateUI stateUIPrefab;
    //public bool isPause { get; private set; }
    private void OnPause(InputValue value)
    {
        Manager.UI.ShowPopUpUI(pauseUIPrefab);
        //isPause = !isPause; // true를 false로 false를 true로
    }
    /*private void OnDisPause()
    {

        isPause = !isPause;
    }*/

    private void OnInventoryOpen(InputValue value) 
    {
        Manager.UI.ShowWindowUI(InventoryUIPrefab);
    }

    private void OnStateOpen(InputValue value)
    {
        Manager.UI.ShowWindowUI(stateUIPrefab);
    }



    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
