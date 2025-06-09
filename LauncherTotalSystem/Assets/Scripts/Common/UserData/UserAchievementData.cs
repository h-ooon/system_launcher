using SuperMaxim.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UserAchievementProgressData
{
    public AchievementType AchievementType;
    public int AchievementAmount;
    public bool IsAchieved;
    public bool IsRewardClaimed;
}

[Serializable]
public class UserAchievementProgressDataListWrapper
{
    public List<UserAchievementProgressData> AchievementProgressDataList;
}

public class AchievementProgressMsg
{

}

public class UserAchievementData : IUserData
{
    public List<UserAchievementProgressData> AchievementProgressDataList { get; set; } = new List<UserAchievementProgressData>();

    public void SetDefaultData()
    {
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;

        try
        {
            string achievementProgressDataListJson = PlayerPrefs.GetString("AchievementProgressDataList");
            if(!string.IsNullOrEmpty(achievementProgressDataListJson))
            {
                UserAchievementProgressDataListWrapper achievementProgressDataListWrapper = JsonUtility.FromJson<UserAchievementProgressDataListWrapper>(achievementProgressDataListJson);
                AchievementProgressDataList = achievementProgressDataListWrapper.AchievementProgressDataList;

                Logger.Log("AchievementProgressDataList");
                foreach (var item in AchievementProgressDataList)
                {
                    Logger.Log($"AchievementType:{item.AchievementType} AchievementAmount:{item.AchievementAmount} IsAchieved:{item.IsAchieved} IsRewardClaimed:{item.IsRewardClaimed}");
                }
            }

            result = true;
        }
        catch (Exception e)
        {
            Logger.Log($"Load failed. (" + e.Message + ")");
        }

        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");

        bool result = false;

        try
        {
            UserAchievementProgressDataListWrapper achievementProgressDataListWrapper = new UserAchievementProgressDataListWrapper();
            achievementProgressDataListWrapper.AchievementProgressDataList = AchievementProgressDataList;
            string achievementProgressDataListJson = JsonUtility.ToJson(achievementProgressDataListWrapper);
            PlayerPrefs.SetString("AchievementProgressDataList", achievementProgressDataListJson);

            Logger.Log("AchievementProgressDataList");
            foreach (var item in AchievementProgressDataList)
            {
                Logger.Log($"AchievementType:{item.AchievementType} AchievementAmount:{item.AchievementAmount} IsAchieved:{item.IsAchieved} IsRewardClaimed:{item.IsRewardClaimed}");
            }
			PlayerPrefs.Save();
            
			result = true;
        }
        catch (Exception e)
        {
            Logger.Log($"Load failed. (" + e.Message + ")");
        }

        return result;
    }

    public UserAchievementProgressData GetUserAchievementProgressData(AchievementType achievementType)
    {
        return AchievementProgressDataList.Where(item => item.AchievementType == achievementType).FirstOrDefault();
    }

    public void ProgressAchievement(AchievementType achievementType, int achieveAmount)
    {
        var achievementData = DataTableManager.Instance.GetAchievementData(achievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist.");
            return;
        }

        UserAchievementProgressData userAchievementProgressData = GetUserAchievementProgressData(achievementType);
        if(userAchievementProgressData == null)
        {
            userAchievementProgressData = new UserAchievementProgressData();
            userAchievementProgressData.AchievementType = achievementType;
            AchievementProgressDataList.Add(userAchievementProgressData);
        }

        if(!userAchievementProgressData.IsAchieved)
        {
            userAchievementProgressData.AchievementAmount += achieveAmount;
            if(userAchievementProgressData.AchievementAmount > achievementData.AchievementGoal)
            {
                userAchievementProgressData.AchievementAmount = achievementData.AchievementGoal;
            }

            if(userAchievementProgressData.AchievementAmount == achievementData.AchievementGoal)
            {
                userAchievementProgressData.IsAchieved = true;
            }

            SaveData();

            var achievementProgressMsg = new AchievementProgressMsg();
            Messenger.Default.Publish(achievementProgressMsg);
        }
    }
}
