using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneData
{
    public List<Data> dataList;
    public List<InventoryData> inventoryDataList;   

    public SceneData(List<Data> dataList, List<InventoryData> inventoryDataList)
    {
        this.dataList = dataList;
        this.inventoryDataList = inventoryDataList;
    }

    public SceneData()
    {
        dataList = new();
        inventoryDataList = new();
    }
}
