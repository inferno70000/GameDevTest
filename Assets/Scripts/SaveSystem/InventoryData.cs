using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryData
{
    public int name;
    public int quantity;

    public InventoryData(int name, int quantity)
    {
        this.name = name;
        this.quantity = quantity;
    }
}
