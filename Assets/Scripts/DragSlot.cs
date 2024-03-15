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

    [SerializeField] Image imageItem;

    private void Start()
    {
        instance = this;
    }


    public void DragSetImage(Image itemImage)
    {
        this.imageItem.sprite = itemImage.sprite;
        SetColor(1);
    }

    public void OutImage(Image itemImage)
    {
        this.imageItem.sprite = itemImage.sprite;
        SetColor(0);
    }

    public void SetColor(float alpha)
    {
        Color color = imageItem.color;
        color.a = alpha;
        imageItem.color = color;
    }

   
}
