using Enums;
using TMPro;
using UnityEditor.TerrainTools;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public QuestSO ActiveQuest
    {
        get => activeQuest; set
        {
            activeQuest = value;
            questName.text = activeQuest.questName;
            questDescription.text = activeQuest.questDescription;
            questReward.text = activeQuest.questRewardGold.ToString();
            string goalItems = null;
            foreach (var item in activeQuest.questGoalItems)
            {
                goalItems += item.Name + ", ";
            }
            questGoalItems.text = goalItems;
        }
    }

    private QuestSO activeQuest;
    public QuestType activeQuestLine;
    public QuestSO[] repeatableQuests;
    public QuestSO[] tutorialQuests;
    [SerializeField] Inventory playerInventory;
    [SerializeField] PlayerData playerData;

    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI questDescription;
    [SerializeField] TextMeshProUGUI questGoalItems;
    [SerializeField] TextMeshProUGUI questReward;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InitializeQuestLine(tutorialQuests, QuestType.Tutorial);
    }

    private void InitializeQuestLine(QuestSO[] questList, QuestType questType)
    {
        activeQuestLine = questType;
        SelectNextQuest(questList);

    }

    private void OnQuestFinished()
    {
        Debug.Log("QuestFinished");
        ActiveQuest.OnQuestFinished -= OnQuestFinished;
        //GiveReward
        playerData.Coin += ActiveQuest.questRewardGold;
        //Give Next Quest
        switch (activeQuestLine)
        {
            case QuestType.Tutorial:
                SelectNextQuest(tutorialQuests);
                break;
            case QuestType.Repeatable:
                SelectNextQuest(repeatableQuests);
                break;
        }

    }
    [ContextMenu(nameof(CompleteQuest))]
    public void CompleteQuest()
    {
        if (IsPlayerHaveThisItems(ActiveQuest.questGoalItems))
        {
            ActiveQuest.OnQuestFinished?.Invoke();
        }
        else Debug.Log("You Dont Have needed item.");
    }

    public void SelectNextQuest(QuestSO[] questList)
    {
        int indxofActiveQuest = GetIndexofActiveQuest(questList);
        if (questList.Length > indxofActiveQuest + 1)
        {
            ActiveQuest = questList[indxofActiveQuest + 1];
            ActiveQuest.OnQuestFinished += OnQuestFinished;
        }
        else if (activeQuestLine != QuestType.Repeatable)
        {
            Debug.Log("Questline Ended.");
            InitializeQuestLine(repeatableQuests, QuestType.Repeatable);
        }

    }

    private int GetIndexofActiveQuest(QuestSO[] questList)
    {
        if (ActiveQuest == null)
        {
            ActiveQuest = questList[0];
            ActiveQuest.OnQuestFinished += OnQuestFinished;
        }

        for (int i = 0; i < questList.Length; i++)
        {
            if (questList[i].questID == ActiveQuest.questID)
            {
                return i;
            }
        }
        return -1;
    }
    public bool IsPlayerHaveThisItems(InventoryItemSO[] goalItemList)
    {
        int accuredItemNumber = 0;
        foreach (InventorySlot iS in playerInventory.inventorySlots)
        {
            if (iS.myItem == null) continue;
            foreach (InventoryItemSO itemData in goalItemList)
            {
                if (iS.myItem.InventoryItemData.ID == itemData.ID)
                {
                    Debug.Log("ItemFinded");
                    accuredItemNumber++;
                }

            }

            if (accuredItemNumber == goalItemList.Length)
            {
                foreach (var item in goalItemList)
                {
                    playerInventory.RemoveItem(item);
                }
                return true;
            }
            else return false;
        }
        return false;
    }
}
