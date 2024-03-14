using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainScene : BaseScene
{
    [SerializeField] PauseUI pauseUIPrefab;
    //[SerializeField] InventoryUI InventoryUIPrefab;

    [SerializeField] InventoryUI inventory;
    [SerializeField] StateUI state;

    bool invenOpen;
    bool stateOpen;

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
        invenOpen = !invenOpen;
        if (invenOpen)
        {
            inventory.transform.position = new Vector3(960, 540, 0);
        }

        else
        {
            inventory.transform.position = new Vector3(1760, 540, 0);
        }
    }

    private void OnStateOpen(InputValue value)
    {
        stateOpen = !stateOpen;
        if (stateOpen)
        {
            state.transform.position = new Vector3(960, 540, 0);
        }

        else
        {
            state.transform.position = new Vector3(0, 540, 0);
        }
    }



    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
