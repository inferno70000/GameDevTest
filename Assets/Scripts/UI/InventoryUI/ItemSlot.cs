using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const string InventoryBarUI = "InventoryBarUI";
    private const string ItemParent = "ItemParent";

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemQuantity;
    [SerializeField] private GameObject itemDragPrefab; //item prefab to show item's image when dragging
    [SerializeField] private GameObject itemDropPrefab; //item prefab to create new item in the scene
    private ItemDetails itemDetails = null; //this item slot's details
    private GameObject itemDrag;
    private Transform inventoryBarUI;
    private Transform itemParent;

    public Image ItemImage { get => itemImage; }
    public TextMeshProUGUI ItemQuantity { get => itemQuantity; }

    private void Start()
    {
        inventoryBarUI = GameObject.FindGameObjectWithTag(InventoryBarUI).transform;
        itemParent = GameObject.FindGameObjectWithTag(ItemParent).transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails != null)
        {
            itemDrag = Instantiate(itemDragPrefab, transform.position, transform.rotation, inventoryBarUI);

            Image itemImage = itemDragPrefab.GetComponent<Image>();

            itemDrag.GetComponent<Image>().color = itemDetails.Color;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemDrag != null)
        {
            itemDrag.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemDrag != null)
        {
            Destroy(itemDrag);

            //If drag ends over on Inventory Bar
            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>() != null)
            {

            }
            //else drop the item
            else
            {
                DropItemAtMousePosition();
            }
        }
    }

    /// <summary>
    /// Drop item at mouse position
    /// </summary>
    private void DropItemAtMousePosition()
    {
        if (itemDetails != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector3 dir = Vector3.zero;

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                dir = hit.point;

                dir.y += 0.5f; //move up item's Y to spawn item at correct position 
            }

            //create item at mouse position and set parent on hierarchy
            GameObject itemDropped = Instantiate(itemDropPrefab, dir, Quaternion.identity, itemParent);

            //set item name to define item's color
            itemDropped.GetComponent<Item>().SetItemName(itemDetails.ItemName);

            //remove item from inventory
            InventoryManager.Instance.RemoveItem(itemDropped.GetComponent<Item>());

            SoundManager.Instance.PlayDropSound();
        }
    }

    public void SetItemDetails(ItemDetails itemDetails)
    {
        this.itemDetails = itemDetails;
    }
}
