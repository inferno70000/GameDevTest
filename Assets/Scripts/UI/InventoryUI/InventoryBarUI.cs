using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBarUI : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> itemSlotList;

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
        //Clear all item slots in the inventory bar
        ClearItemSlot();

        //if item slot and inventory item list are not empty, update inventory item
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
                        itemSlotList[i].SetItemDetails(itemDetails);
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
                itemSlotList[i].SetItemDetails(null);
            }
        }
    }
}
