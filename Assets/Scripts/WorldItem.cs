using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour, IPickupable, IInteractable
{
    [SerializeField] InventoryItemSO _inventoryItemData;
    public InventoryItemSO InventoryItemData { get => _inventoryItemData; set => _inventoryItemData = value; }
    [HideInInspector] public Transform t => transform;
}
