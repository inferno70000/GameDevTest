using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{
    public Action<List<InventoryItem>> OnInventoryUpdate;

    [SerializeField] private ItemDetailsListSO itemDetailsListSO;
    private List<InventoryItem> inventoryList;
    [SerializeField] private GameObject itemDragPrefab;
    private GameObject itemDrag;
    protected override void Awake()
    {
        base.Awake();

        inventoryList = new();
    }

    /// <summary>
    /// if inventory list is empty, create 10 items for each type
    /// </summary>
    public void CreateInventoryList()
    {
        if (inventoryList.Count == 0)
        {
            for (int i = 0; i < 10; i++)
            {
                AddItem(ItemName.Red);
                AddItem(ItemName.Blue);
                AddItem(ItemName.Pink);
            }
        }
    }

    /// <summary>
    /// Add an item by item name
    /// </summary>
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

    public List<InventoryItem> GetInventoryItemList()
    {
        return inventoryList;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //fire a ray at mouse position
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                //if ray hit an item, add item and destroy item in the scene
                if (hit.collider.TryGetComponent<Item>(out var item))
                {
                    SoundManager.Instance.PlayPickSound();

                    AddItem(item.ItemName);

                    Destroy(item.gameObject);
                }
            }

        }
    }
}
