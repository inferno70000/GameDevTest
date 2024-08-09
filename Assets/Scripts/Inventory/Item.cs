using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemName itemName;
    private Renderer itemRenderer;

    public ItemName ItemName { get => itemName; }

    private void Awake()
    {
        itemRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        //Set item color by item name
        InitItem();
    }

    private void InitItem()
    {
        switch (itemName)
        {
            case ItemName.Red:
                itemRenderer.material.color = Color.red;
                break;
            case ItemName.Blue:
                itemRenderer.material.color = Color.blue;
                break;
            case ItemName.Pink:
                itemRenderer.material.color = new(255, 0, 255, 255);
                break;
            default:
                break;
        }
    }

    public void SetItemName(ItemName itemName)
    {
        this.itemName = itemName;
    }
}
