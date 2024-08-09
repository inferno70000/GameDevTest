using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemQuantity;

    public Image ItemImage { get => itemImage; }
    public TextMeshProUGUI ItemQuantity { get => itemQuantity; }
}
