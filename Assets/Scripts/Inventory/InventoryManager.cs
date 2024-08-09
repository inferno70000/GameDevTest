using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{
    public Action<List<InventoryItem>> OnInventoryUpdate;

    [SerializeField] private ItemDetailsListSO itemDetailsListSO;
    private Dictionary<string, ItemDetails> itemDetailsDictionary = new();
    private List<InventoryItem> inventoryList;

    protected override void Awake()
    {
        base.Awake();

        CreateInventory();
    }

    private void CreateInventory()
    {
        inventoryList = new();

        foreach (var item in itemDetailsListSO.itemDetailsList)
        {
            itemDetailsDictionary.Add(item.ItemName.ToString(), item);
        }
    }

    /// <summary>
    /// Add an item by item name
    /// </summary>
    /// <param name="itemName"></param>
    public void AddItem(ItemName itemName)
    {
        int itemPosition = FindPositionItem(itemName);

        //if already have the same item, increase the item's quantity
        if (itemPosition != -1)
        {
            AddItemByPosition(itemName, itemPosition);
        }
        //if it's a new item, add that item to the inventory
        else
        {
            AddNewItem(itemName);
        }

        OnInventoryUpdate?.Invoke(inventoryList);
    }

    /// <summary>
    /// Add an item if that item is existed
    /// </summary>
    private void AddItemByPosition(ItemName itemName, int itemPosition)
    {
        InventoryItem inventoryItem = new();

        inventoryItem.ItemName = itemName;
        inventoryItem.ItemQuantity = inventoryList[itemPosition].ItemQuantity + 1;
        inventoryList[itemPosition] = inventoryItem;

        //PrintPickedUpItem(inventoryItem);
    }

    /// <summary>
    /// Add a new item
    /// </summary>
    /// <param name="itemName"></param>
    private void AddNewItem(ItemName itemName)
    {
        InventoryItem inventoryItem = new();

        inventoryItem.ItemName = itemName;
        inventoryItem.ItemQuantity = 1;
        inventoryList.Add(inventoryItem);

        //PrintPickedUpItem(inventoryItem);
    }

    /// <summary>
    /// Find the item by item's name. If the item is existed in the inventory, return the item's position else return -1
    /// </summary>
    public int FindPositionItem(ItemName itemName)
    {
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].ItemName == itemName)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Remove item with parameter is Item
    /// </summary>
    public void RemoveItem(Item item)
    {
        ItemName itemName = item.ItemName;

        RemoveItem(itemName);
    }

    /// <summary>
    /// Remove item with parameter is item' name
    /// </summary>
    /// <param name="itemName"></param>
    public void RemoveItem(ItemName itemName)
    {
        int itemPosition = FindPositionItem(itemName);

        if (itemPosition != -1)
        {
            int quantity = inventoryList[itemPosition].ItemQuantity;
            if (quantity > 1)
            {
                inventoryList[itemPosition].ItemQuantity -= 1;
            }
            else
            {
                inventoryList.RemoveAt(itemPosition);
            }
        }

        OnInventoryUpdate?.Invoke(inventoryList);
    }

    /// <summary>
    /// Get item's details from item details list SO
    /// </summary>
    public ItemDetails GetItemDetailsByName(ItemName itemName)
    {
        return itemDetailsListSO.GetItemDetailsByName(itemName);
    }

    private void PrintPickedUpItem(InventoryItem inventoryItem)
    {
        Debug.Log(inventoryItem.ItemName + " - " + inventoryItem.ItemQuantity);
    }
}
