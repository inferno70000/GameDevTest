using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item details list SO")]
public class ItemDetailsListSO : ScriptableObject
{
    public List<ItemDetails> itemDetailsList;

    /// <summary>
    /// Get item's details from itemDetailsList 
    /// </summary>
    public ItemDetails GetItemDetailsByName(ItemName itemName)
    {
        return itemDetailsList.Find(x => x.ItemName == itemName);  
    }
}
