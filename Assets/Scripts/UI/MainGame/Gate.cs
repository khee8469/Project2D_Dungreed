using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteractor player)
    {
        Manager.Scene.LoadScene("Title");
    }
}
