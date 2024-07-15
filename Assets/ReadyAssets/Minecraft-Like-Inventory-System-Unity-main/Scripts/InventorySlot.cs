using UnityEngine;
using UnityEngine.EventSystems;
using Enums;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem myItem { get; set; }
    private Inventory _inventory;
    public Inventory inventory
    {
        get
        {
            if (_inventory == null) _inventory = FindObjectOfType<Inventory>();
            return _inventory;
        }
    }


    public InventorySlotType mySlotType;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (inventory.carriedItem == null) return;
            if (mySlotType == InventorySlotType.Drop) { DropItem(inventory.carriedItem); return; }
            SetItem(inventory.carriedItem);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (inventory.carriedItem == null) return;
            if (mySlotType == InventorySlotType.Drop) { DropStack(inventory.carriedItem); return; }
        }
    }

    public void SetItem(InventoryItem item)
    {
        inventory.carriedItem = null;

        // Reset old slot
        item.activeSlot.myItem = null;

        // Set current slot
        myItem = item;
        myItem.activeSlot = this;
        myItem.transform.SetParent(transform);
        myItem.canvasGroup.blocksRaycasts = true;
    }
    public void DropItem(InventoryItem item)
    {
        Debug.Log(item.InventoryItemData.name + " dropped.");
        //Instantiate in World.
        GameObject spawnedObject = Instantiate(item.InventoryItemData.itemPrefab);
        spawnedObject.transform.position = Camera.main.transform.position;
        item.ItemCount--;
        //Remove Item Prefab
        if (item.ItemCount <= 0)
        {
            inventory.carriedItem = null;
            item.activeSlot.myItem = null;
            Destroy(item.gameObject);
        }
    }
    public void DropStack(InventoryItem item)
    {
        for (int i = 0; i < item.ItemCount; i++)
        {
            GameObject spawnedObject = Instantiate(item.InventoryItemData.itemPrefab);
            spawnedObject.transform.position = Camera.main.transform.position;
        }
        inventory.carriedItem = null;
        item.activeSlot.myItem = null;
        Destroy(item.gameObject);
    }
}
