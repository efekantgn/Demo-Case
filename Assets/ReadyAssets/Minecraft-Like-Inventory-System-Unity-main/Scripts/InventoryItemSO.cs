using UnityEngine;
using Enums;

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class InventoryItemSO : ScriptableObject
{
    public int ID;
    public string Name;
    public Sprite sprite;
    public ItemType itemType;
    public int maxStackCount = 1;
    public GameObject itemPrefab;
    public string description;
    public int price;
}
