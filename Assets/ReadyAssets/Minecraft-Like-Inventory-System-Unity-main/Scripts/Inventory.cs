using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public InventoryItem carriedItem;

    public InventorySlot[] inventorySlots;
    [SerializeField] InventorySlot[] hotbarSlots;
    public GameObject SelectedHotbarFrame;
    [SerializeField] InventorySlot SelectedHotbar;
    int SelectedHotbarIndex = 0;
    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    [Header("Item List")]
    [SerializeField] InventoryItemSO[] items;

    [Header("Debug")]
    [SerializeField] Button giveItemBtn;


    void Awake()
    {
        giveItemBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
    }

    void Update()
    {
        if (carriedItem == null) return;
        carriedItem.transform.position = Input.mousePosition;
    }

    public void SetCarriedItem(InventoryItem item)
    {
        if (carriedItem != null)
        {
            item.activeSlot.SetItem(carriedItem);
        }

        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
    }

    public bool SpawnInventoryItem(InventoryItemSO item = null)
    {
        InventoryItemSO _item = item;
        if (_item == null)
        { _item = PickRandomItem(); }

        int LastEmptyIndex = -1;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].myItem != null && inventorySlots[i].myItem.InventoryItemData.ID == _item.ID && inventorySlots[i].myItem.ItemCount < _item.maxStackCount)
            {
                inventorySlots[i].myItem.ItemCount++;
                inventorySlots[i].myItem.ItemCountText.text = inventorySlots[i].myItem.ItemCount.ToString();
                return true;
            }
            if (inventorySlots[i].myItem == null) LastEmptyIndex = i;
        }
        if (LastEmptyIndex != -1)
        {
            Instantiate(itemPrefab, inventorySlots[LastEmptyIndex].transform).Initialize(_item, inventorySlots[LastEmptyIndex], this);
            return true;
        }
        return false;
    }

    InventoryItemSO PickRandomItem()
    {
        int random = Random.Range(0, items.Length);
        return items[random];
    }

    public void ChangeSelectedHotbar(float value)
    {
        if (value > 0)
        {
            SelectedHotbarIndex++;
            SelectedHotbarIndex = SelectedHotbarIndex % 9;
        }
        else if (value < 0)
        {
            SelectedHotbarIndex--;
            if (SelectedHotbarIndex < 0) SelectedHotbarIndex = hotbarSlots.Length - 1;
        }
        SelectedHotbar = hotbarSlots[SelectedHotbarIndex];
        SelectedHotbarFrame.transform.position = SelectedHotbar.transform.position;
    }

    public void UseSelectedHotbar()
    {
        if (SelectedHotbar.myItem == null) return;
        SelectedHotbar.myItem.Activate();
    }

    public void RemoveItem(InventoryItemSO iiSO)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].myItem == null) continue;

            if (inventorySlots[i].myItem.InventoryItemData.ID == iiSO.ID)
            {
                inventorySlots[i].myItem.ItemCount--;
                if (inventorySlots[i].myItem.ItemCount <= 0)
                {
                    Destroy(inventorySlots[i].myItem.gameObject);
                }
                break;
            }

        }
    }


}
