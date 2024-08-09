using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemName itemName;

    public ItemName ItemName { get => itemName; }

    private void Start()
    {
        InitItem();
    }

    private void InitItem()
    {
        switch (itemName)
        {
            case ItemName.Red:
                break;
            case ItemName.Blue:
                break;
            case ItemName.Pink:
                break;
            default:
                break;
        }
    }
}
