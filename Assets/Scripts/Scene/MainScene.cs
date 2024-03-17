using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainScene : BaseScene
{
    [SerializeField] PauseUI pauseUIPrefab;
    //[SerializeField] InventoryUI InventoryUIPrefab;

    [SerializeField] GameObject inventory;
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

        
        //isPause = !isPause; // true�� false�� false�� true��
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
            //inventory.transform.position = new Vector3(960, 540, 0);
            inventory.gameObject.SetActive(true);
        }

        else
        {
            //inventory.transform.position = new Vector3(1760, 540, 0);
            inventory.gameObject.SetActive(false);
        }
    }

    private void OnStateOpen(InputValue value)
    {
        stateOpen = !stateOpen;
        if (stateOpen)
        {
            state.gameObject.SetActive(true);
        }

        else
        {
            state.gameObject.SetActive(false);
        }
    }



    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
