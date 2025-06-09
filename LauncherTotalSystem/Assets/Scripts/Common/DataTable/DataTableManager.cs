using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GlobalDefine;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTable";


    protected override void Init()
    {
        base.Init();

        LoadChapterDataTable();
        LoadItemDataTable();
        LoadAchievementDataTable();
    }

    #region CHAPTER_DATA
    private const string CHAPTER_DATA_TABLE = "ChapterDataTable";
    private List<ChapterData> ChapterDataTable = new List<ChapterData>();

    private void LoadChapterDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{CHAPTER_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var chapterData = new ChapterData
            {
                ChapterNo = Convert.ToInt32(data["chapter_no"]),
                ChapterName = data["chapter_name"].ToString(),
                TotalStages = Convert.ToInt32(data["total_stages"]),
                ChapterRewardGem = Convert.ToInt32(data["chapter_reward_gem"]),
                ChapterRewardGold = Convert.ToInt32(data["chapter_reward_gold"]),
            };

            ChapterDataTable.Add(chapterData);
        }
    }

    public ChapterData GetChapterData(int chapterNo)
    {
        return ChapterDataTable.Where(item => item.ChapterNo == chapterNo).FirstOrDefault();
    }
    #endregion

    #region ITEM_DATA
    private const string ITEM_DATA_TABLE = "ItemDataTable";
    private List<ItemData> ItemDataTable = new List<ItemData>();

    private void LoadItemDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{ITEM_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var itemData = new ItemData
            {
                ItemId = Convert.ToInt32(data["item_id"]),
                ItemName = data["item_name"].ToString(),
                AttackPower = Convert.ToInt32(data["attack_power"]),
                Defense = Convert.ToInt32(data["defense"]),
            };

            ItemDataTable.Add(itemData);
        }
    }

    public ItemData GetItemData(int itemId)
    {
        return ItemDataTable.Where(item => item.ItemId == itemId).FirstOrDefault();
    }
    #endregion

    #region ACHIEVEMENT_DATA
    private const string ACHIEVEMENT_DATA_TABLE = "AchievementDataTable";
    private List<AchievementData> AchievementDataTable = new List<AchievementData>();
    
    public List<AchievementData> GetAchievementDataList()
    {
        return AchievementDataTable;
    }

    private void LoadAchievementDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{ACHIEVEMENT_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var achievementData = new AchievementData
            {
                AchievementType = (AchievementType)Enum.Parse(typeof(AchievementType), data["achievement_type"].ToString()),
                AchievementName = data["achievement_name"].ToString(),
                AchievementGoal = Convert.ToInt32(data["achievement_goal"]),
                AchievementRewardType = (RewardType)Enum.Parse(typeof(RewardType), data["achievement_reward_type"].ToString()),
                AchievementRewardAmount = Convert.ToInt32(data["achievement_reward_amount"])
            };

            AchievementDataTable.Add(achievementData);
        }
    }

    public AchievementData GetAchievementData(AchievementType achievementType)
    {
        return AchievementDataTable.Where(item => item.AchievementType == achievementType).FirstOrDefault();
    }
    #endregion
}