using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Enums;
public class ShopItemContainer : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Count;
    public TextMeshProUGUI BuyOrSellText;
    public InventoryItemSO inventoryItemSO;
    public Shop shop;
    private ShopType shopType = ShopType.None;

    public void SetShopItem(InventoryItemSO iiSO, int sellAmount, ShopType st, Shop s)
    {
        Name.text = iiSO.Name;
        Count.text = sellAmount.ToString();
        Icon.sprite = iiSO.sprite;
        inventoryItemSO = iiSO;
        shopType = st;
        shop = s;
        switch (shopType)
        {
            case ShopType.Buy:
                BuyOrSellText.text = "Buy";
                break;
            case ShopType.Sell:
                BuyOrSellText.text = "Sell";
                break;
            default:
                break;
        }
    }

    public void OnButtonClicked()
    {
        switch (shopType)
        {
            case ShopType.Buy:
                shop.BuyItem?.Invoke(inventoryItemSO);
                break;
            case ShopType.Sell:
                shop.SellItem?.Invoke(inventoryItemSO);
                break;
            default:
                break;
        }

    }
}
