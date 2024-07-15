
using UnityEngine;

public interface IPickupable
{
    public InventoryItemSO InventoryItemData { get; set; }
    public Transform t { get; }
}
