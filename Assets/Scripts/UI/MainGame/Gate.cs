using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteractor player)
    {
        Manager.Data.SaveData();
        Manager.Scene.LoadScene("Title");
    }
}
