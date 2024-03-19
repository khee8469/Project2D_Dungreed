using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] TextUI textUI;
    public void Interact(PlayerInteractor player)
    {
        Manager.UI.ShowPopUpUI(textUI);
        //Time.timeScale = 1f;
        Debug.Log("¥Î»≠");
    }
}
