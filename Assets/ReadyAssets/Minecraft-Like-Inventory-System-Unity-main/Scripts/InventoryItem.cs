using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Enums;
public class InventoryItem : MonoBehaviour, IPointerClickHandler, IActivatable
{
    [SerializeField] Image itemIcon;
    public CanvasGroup canvasGroup;
    private int itemCount = 0;
    public InventoryItemSO InventoryItemData { get; set; }
    public InventorySlot activeSlot { get; set; }
    public int ItemCount
    {
        get => itemCount;
        set
        {
            itemCount = value;
            ItemCountText.text = itemCount.ToString();
        }
    }


    public ItemType itemType = ItemType.None;
    public TextMeshProUGUI ItemCountText;
    public Inventory inventory;

    public void Initialize(InventoryItemSO itemData, InventorySlot parent, Inventory i)
    {
        inventory = i;
        activeSlot = parent;
        activeSlot.myItem = this;
        itemType = itemData.itemType;
        ItemCount = 1;
        InventoryItemData = itemData;
        itemIcon.sprite = itemData.sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventory.SetCarriedItem(this);
        }
    }

    public void Activate()
    {
        switch (itemType)
        {
            case ItemType.None:
                break;
            case ItemType.Seed:
                PlantSeed();
                break;
            case ItemType.Tool:
                UseTool();
                break;
            default:
                break;
        }
    }

    private void UseTool()
    {
        Tool tool = InventoryItemData.itemPrefab.GetComponent<Tool>();
        //Wait for tool usage time and continue
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2) &&
            hit.transform.TryGetComponent(out Seed s))
        {
            s.HarvestCrop(tool.efficiency);
        }
    }

    private void PlantSeed()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2) &&
            hit.transform.TryGetComponent(out Farmland f))
        {
            f.PlantSeed(InventoryItemData, hit.point);
        }
        else
        {
            Debug.Log("Seeds Can be used on farmlands.");
            return;
        }
        ItemCount--;

        if (ItemCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
