using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class MainScene : BaseScene
{
    [SerializeField] PauseUI pauseUIPrefab;
    //[SerializeField] InventoryUI InventoryUIPrefab;
    [SerializeField] StateUI state;

    //public PlayerMove player;
    public HpBar hpBar;
    //public InventoryUI inventoryUI;


    private void Start()
    {
        pauseUIPrefab = FindObjectOfType<PauseUI>();
        hpBar = FindObjectOfType<HpBar>();
        //inventoryUI = FindObjectOfType<InventoryUI>();
        hpBar.SetHp(Manager.Data.GameData.hp, Manager.Data.GameData.maxHp);
    }



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

    /*private void OnInventoryOpen(InputValue value)
    {
        Debug.Log(inventory.activeSelf);
        if (!inventory.activeSelf)
        {
            //inventory.transform.position = new Vector3(960, 540, 0);
            inventory.gameObject.SetActive(true);
        }

        else
        {
            //inventory.transform.position = new Vector3(1760, 540, 0);
            inventory.gameObject.SetActive(false);
        }
    }*/

    /*private void OnStateOpen(InputValue value)
    {
        //stateOpen = !stateOpen;
        if (!state.gameObject.activeSelf)
        {
            state.gameObject.SetActive(true);
        }

        else
        {
            state.gameObject.SetActive(false);
        }
    }*/



    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
