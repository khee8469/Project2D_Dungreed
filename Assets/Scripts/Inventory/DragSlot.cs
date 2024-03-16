using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;

    public Slot dragSlot;

    public Image itemImage;

    private void Start()
    {
        itemImage = GetComponent<Image>();
        instance = this;
    }


    public void DragSlotSetImage(Sprite itemImage)
    {
        this.itemImage.sprite = itemImage;
        SetColor(1);
    }

    public void DragSlotClear()
    {
        this.itemImage.sprite = null;
        dragSlot = null;
        SetColor(0);
    }

    public void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

   
}
