using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewQuest", menuName = "CreateQuest")]
public class QuestSO : ScriptableObject
{
    public int questID;
    public string questName;
    [TextArea] public string questDescription;
    public InventoryItemSO[] questGoalItems;
    public int questRewardGold;
    public Action OnQuestFinished;
}

