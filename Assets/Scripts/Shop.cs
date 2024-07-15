using System;
using UnityEngine;
using Enums;
public class Shop : MonoBehaviour
{
    [SerializeField] InventoryItemSO[] InventoryItemSOs;
    [SerializeField] ShopItemContainer ShopItemContainer;
    [SerializeField] Transform BuyContentArea;
    [SerializeField] Transform SellContentArea;
    [SerializeField] Inventory PlayerInventory;
    [SerializeField] PlayerData playerData;
    public Action<InventoryItemSO> BuyItem = null;
    public Action<InventoryItemSO> SellItem = null;

    private void OnEnable()
    {
        BuyItem += BuyShopItem;
        SellItem += SellInventoryItem;
    }
    private void OnDisable()
    {
        BuyItem -= BuyShopItem;
        SellItem -= SellInventoryItem;
    }
    public void InitializeShop()
    {
        ClearShop(BuyContentArea);
        foreach (InventoryItemSO s in InventoryItemSOs)
        {
            ShopItemContainer sic = Instantiate(ShopItemContainer, BuyContentArea);
            sic.SetShopItem(s, s.maxStackCount, ShopType.Buy, this);
        }
    }

    public void InitalizeInventory()
    {
        ClearShop(SellContentArea);
        foreach (InventorySlot IS in PlayerInventory.inventorySlots)
        {
            if (IS.myItem != null && IS.myItem.ItemCount > 0)
            {
                ShopItemContainer sic = Instantiate(ShopItemContainer, SellContentArea);
                sic.SetShopItem(IS.myItem.InventoryItemData, IS.myItem.ItemCount, ShopType.Sell, this);
            }
        }

    }

    public void ClearShop(Transform shop)
    {
        foreach (Transform item in shop)
        {
            Destroy(item.gameObject);
        }
    }

    public void BuyShopItem(InventoryItemSO iiSO)
    {
        if (playerData.Coin < iiSO.price)
        {
            Debug.Log("Dont Have Enough Gold");
            return;
        }
        else
        {
            if (!PlayerInventory.SpawnInventoryItem(iiSO))
            {
                Debug.Log("Dont Have Enoug Space");
                return;
            }
            playerData.Coin -= iiSO.price;
        }

    }

    public void SellInventoryItem(InventoryItemSO iiSO)
    {
        PlayerInventory.RemoveItem(iiSO);
        playerData.Coin += iiSO.price;
        InitalizeInventory();
    }

}
