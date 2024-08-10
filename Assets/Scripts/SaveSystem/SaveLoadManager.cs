using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class SaveLoadManager : SingletonMonoBehaviour<SaveLoadManager>
{
    [SerializeField] private GameObject itemPrefab;
    private Transform itemParent;

    private void Start()
    {
        itemParent = GameObject.FindGameObjectWithTag("ItemParent").transform;
        LoadData(); //load data when the game start
    }

    private void Update()
    {
        //if input is Escape key, save data and quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveData();
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
                Application.Quit();
#endif
        }
    }

    /// <summary>
    /// Save scene item data and inventory item data
    /// </summary>
    public void SaveData()
    {
        Item[] itemList = FindObjectsOfType<Item>();    //get all Item compents in game
        SceneData sceneData = new();    //create new scene data

        //loop through all item scene in game
        foreach (Item item in itemList)
        {
            Data data = new(item.transform.position, (int)item.ItemName);
            sceneData.dataList.Add(data);
        }

        List<InventoryItem> inventoryItemList = InventoryManager.Instance.GetInventoryItemList();
        //loop through all inventory item
        foreach (InventoryItem item in inventoryItemList)
        {
            InventoryData inventoryData = new((int)item.ItemName, item.ItemQuantity);
            sceneData.inventoryDataList.Add(inventoryData);
        }

        #region Write file
        BinaryFormatter fomatter = new();

        string path = Application.persistentDataPath + "/GameDevTest.data";
        FileStream fileStream = new(path, FileMode.Create);

        fomatter.Serialize(fileStream, sceneData);
        fileStream.Close();
        #endregion
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/GameDevTest.data";
        //if data file existed, load data from that file
        if (File.Exists(path))
        {
            //clear all item in the scene
            ClearAllItem();

            #region Read file
            BinaryFormatter fomatter = new();
            FileStream fileStream = new(path, FileMode.Open);

            SceneData sceneData = fomatter.Deserialize(fileStream) as SceneData;

            //Initiate item in the scene
            if (sceneData.dataList != null)
            {
                foreach (Data data in sceneData.dataList)
                {
                    Vector3 position = new(data.position[0], data.position[1], data.position[2]);
                    ItemName itemName = (ItemName)data.name;

                    GameObject item = Instantiate(itemPrefab, position, Quaternion.identity, itemParent);
                    item.GetComponent<Item>().SetItemName(itemName);
                }
            }
            //Initiate item in the inventory
            if (sceneData.inventoryDataList != null)
            {
                foreach (InventoryData inventoryData in sceneData.inventoryDataList)
                {
                    ItemName itemName = (ItemName)inventoryData.name;

                    for (int i = 0; i < inventoryData.quantity; i++)
                    {
                        InventoryManager.Instance.AddItem(itemName);
                    }
                }
            }
            #endregion
        }
        else
        {
            Debug.LogWarning("Save file not found");
            InventoryManager.Instance.CreateInventoryList();
        }

    }

    /// <summary>
    /// Clear all item in the scene
    /// </summary>
    private void ClearAllItem()
    {
        Item[] itemList = FindObjectsOfType<Item>();

        foreach (Item item in itemList)
        {
            Destroy(item.gameObject);
        }
    }
}
