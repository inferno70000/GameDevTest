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
    private Transform itemParent;
    private Item itemDropped;

    public Image ItemImage { get => itemImage; }
    public TextMeshProUGUI ItemQuantity { get => itemQuantity; }

    private void Start()
    {
        itemParent = GameObject.FindGameObjectWithTag(ItemParent).transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails != null)
        {
            itemDrag = Instantiate(itemDragPrefab);

            itemDropped = itemDrag.GetComponent<Item>();

            itemDrag.transform.SetParent(itemParent, false);

            itemDropped.SetItemName(itemDetails.ItemName);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemDrag != null)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector3 dir = Vector3.zero;

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                dir = hit.point;

                //move up item's Y to spawn item at correct position on the plane
                dir.y = 0.5f; 
            }

            Vector3 collisionPos = itemDrag.GetComponent<Item>().CollisionPosition;

            //if collider's position != zero & the distance between collider and mouse not over 1f
            if (collisionPos != Vector3.zero && 
                collisionPos.x < dir.x + 1 && collisionPos.x > dir.x - 1 &&
                collisionPos.z < dir.z + 1 && collisionPos.z > dir.z - 1)
            {
                dir = collisionPos;
                dir.y += 1f;
            }
            //else set back to position on the plane
            else
            {
                dir = hit.point;

                dir.y = 0.5f;
            }

            itemDrag.transform.SetPositionAndRotation(dir, Quaternion.identity);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemDrag != null)
        {
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
            Vector3 itemDragPosition = itemDrag.transform.position;

            //stop trigger between 2 item
            itemDragPosition.y += 0.001f; 

            itemDrag.transform.position = itemDragPosition;

            //remove item from inventory
            InventoryManager.Instance.RemoveItem(itemDropped.GetComponent<Item>());

            //play drop sound
            SoundManager.Instance.PlayDropSound();
        }
    }

    public void SetItemDetails(ItemDetails itemDetails)
    {
        this.itemDetails = itemDetails;
    }
}
