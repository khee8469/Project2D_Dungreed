using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class MainScene : BaseScene
{
    [SerializeField] PauseUI pauseUIPrefab;
    [SerializeField] StateUI state;
    [SerializeField] GameObject inventory;

    //public PlayerMove player;
    public HpBar hpBar;
    

    private void Start()
    {
        hpBar = FindObjectOfType<HpBar>();
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
    }


    private void OnInventoryOpen(InputValue value)
    {
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
    }

    private void OnStateOpen(InputValue value)
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
    }



    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
