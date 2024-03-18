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

    public Image slotImage;
    /*public ItemType itemType;
    public int itemId;*/

    private void Start()
    {
        instance = this;
        slotImage = GetComponent<Image>();
    }


    public void DragSlotSetImage(Sprite itemImage)
    {
        this.slotImage.sprite = itemImage;
        SetColor(1);
    }

    public void DragSlotClear()
    {
        this.slotImage.sprite = null;
        dragSlot = null;
        SetColor(0);
    }

    public void SetColor(float alpha)
    {
        Color color = slotImage.color;
        color.a = alpha;
        slotImage.color = color;
    }

   
}
