using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCubeButton : MonoBehaviour
{
    [SerializeField] private ItemName itemName;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            //Add 10 cube to inventory
            for (int i = 0; i < 10; i++)
            {
                InventoryManager.Instance.AddItem(itemName);
            }
        });
    }
}
