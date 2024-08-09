using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBarUI : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> itemSlotList;

    private void Start()
    {
        //CreateItemSlotList();
    }

    //private void CreateItemSlotList()
    //{
    //    itemSlotList = new();

    //    foreach (Transform item in transform)
    //    {
    //        ItemSlot itemSlot = item.GetComponent<ItemSlot>();

    //        itemSlotList.Add(itemSlot);
    //    }
    //}

    private void OnEnable()
    {
        InventoryManager.Instance.OnInventoryUpdate += InventoryManager_OnInventoryUpdated;
    }

    private void OnDisable()
    {
        InventoryManager.Instance.OnInventoryUpdate -= InventoryManager_OnInventoryUpdated;
    }

    private void InventoryManager_OnInventoryUpdated(List<InventoryItem> inventoryItemList)
    {

        ClearItemSlot();

        if (itemSlotList.Count > 0 && inventoryItemList.Count > 0)
        {
            for (int i = 0; i < itemSlotList.Count; i++)
            {
                if (i < inventoryItemList.Count)
                {
                    ItemName itemName = inventoryItemList[i].ItemName;

                    ItemDetails itemDetails = InventoryManager.Instance.GetItemDetailsByName(itemName);

                    if (itemDetails != null)
                    {
                        itemSlotList[i].ItemImage.color = itemDetails.Color;
                        itemSlotList[i].ItemQuantity.text = inventoryItemList[i].ItemQuantity.ToString();
                    }
                }
                else
                {
                    break;
                }
            }
        }

    }

    private void ClearItemSlot()
    {
        if (itemSlotList.Count > 0)
        {
            for (int i = 0; i < itemSlotList.Count; i++)
            {
                itemSlotList[i].ItemImage.color = Color.white;
                itemSlotList[i].ItemQuantity.text = "";
            }
        }
    }
}
