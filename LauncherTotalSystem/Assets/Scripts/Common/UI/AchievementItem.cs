using Gpm.Ui;
using SuperMaxim.Messaging;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItemData : InfiniteScrollData
{
    public AchievementType AchievementType;
    public int AchieveAmount;
    public bool IsAchieved;
    public bool IsRewardClaimed;
}

public class AchievementItem : InfiniteScrollItem
{
    public GameObject AchievedBg;
    public GameObject UnAchievedBg;
    public TextMeshProUGUI AchievementNameTxt;
    public Slider AchievementProgressSlider;
    public TextMeshProUGUI AchievementProgressTxt;
    public Image RewardIcon;
    public TextMeshProUGUI RewardAmountTxt;
    public Button ClaimBtn;
    public Image ClaimBtnImg;
    public TextMeshProUGUI ClaimBtnTxt;

    private AchievementItemData m_AchievementItemData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        m_AchievementItemData = scrollData as AchievementItemData;
        if(m_AchievementItemData == null)
        {
            Logger.LogError("m_AchievementItemData is invalid.");
            return;
        }

        var achievementData = DataTableManager.Instance.GetAchievementData(m_AchievementItemData.AchievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist.");
            return;
        }

        AchievedBg.SetActive(m_AchievementItemData.IsAchieved);
        UnAchievedBg.SetActive(!m_AchievementItemData.IsAchieved);
        AchievementNameTxt.text = achievementData.AchievementName;
        AchievementProgressSlider.value = (float)m_AchievementItemData.AchieveAmount / achievementData.AchievementGoal;
        AchievementProgressTxt.text = $"{m_AchievementItemData.AchieveAmount.ToString("N0")}/{achievementData.AchievementGoal.ToString("N0")}";
        RewardAmountTxt.text = achievementData.AchievementRewardAmount.ToString("N0");

        var rewardTextureName = string.Empty;
        switch (achievementData.AchievementRewardType)
        {
            case GlobalDefine.RewardType.Gold:
                rewardTextureName = "IconGolds";
                break;
            case GlobalDefine.RewardType.Gem:
                rewardTextureName = "IconGems";
                break;
            default:
                break;
        }

        var rewardTexture = Resources.Load<Texture2D>($"Textures/{rewardTextureName}");
        if(rewardTexture != null)
        {
            RewardIcon.sprite = Sprite.Create(rewardTexture, new Rect(0, 0, rewardTexture.width, rewardTexture.height), new Vector2(1f, 1f));
        }

        ClaimBtn.enabled = m_AchievementItemData.IsAchieved && !m_AchievementItemData.IsRewardClaimed;
        ClaimBtnImg.color = ClaimBtn.enabled ? Color.white : Color.grey;
        ClaimBtnTxt.color = ClaimBtn.enabled ? Color.white : Color.grey;
    }

    public void OnClickClaimBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        if (!m_AchievementItemData.IsAchieved || m_AchievementItemData.IsRewardClaimed)
        {
            return;
        }

        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData == null)
        {
            Logger.LogError("UserAchievementData does not exist.");
            return;
        }

        var achievementData = DataTableManager.Instance.GetAchievementData(m_AchievementItemData.AchievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist.");
            return;
        }

        var userAchievedData = userAchievementData.GetUserAchievementProgressData(m_AchievementItemData.AchievementType);
        if(userAchievedData != null)
        {
            var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
            if(userGoodsData != null)
            {
                userAchievedData.IsRewardClaimed = true;
                userAchievementData.SaveData();
                m_AchievementItemData.IsRewardClaimed = true;

                switch (achievementData.AchievementRewardType)
                {
                    case GlobalDefine.RewardType.Gold:
                        userGoodsData.Gold += achievementData.AchievementRewardAmount;
                        var goldUpdateMsg = new GoldUpdateMsg();
                        goldUpdateMsg.isAdd = true;
                        Messenger.Default.Publish(goldUpdateMsg);
                        userAchievementData.ProgressAchievement(AchievementType.CollectGold, achievementData.AchievementRewardAmount);
                        break;
                    case GlobalDefine.RewardType.Gem:
                        userGoodsData.Gem += achievementData.AchievementRewardAmount;
                        var gemUpdateMsg = new GemUpdateMsg();
                        gemUpdateMsg.isAdd = true;
                        Messenger.Default.Publish(gemUpdateMsg);
                        break;
                    default:
                        break;
                }

                userGoodsData.SaveData();
            }
        }
    }
}
