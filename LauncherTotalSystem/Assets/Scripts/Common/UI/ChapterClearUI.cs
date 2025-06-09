using SuperMaxim.Messaging;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterClearUIData : BaseUIData
{
    public int chapter;
    public bool earnReward;
}

public class ChapterClearUI : BaseUI
{
    public GameObject Rewards;
    public TextMeshProUGUI GemRewardAmountTxt;
    public TextMeshProUGUI GoldRewardAmountTxt;
    public Button HomeBtn;
    public ParticleSystem[] ClearFX;

    private ChapterClearUIData m_ChapterClearUIData;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        m_ChapterClearUIData = uiData as ChapterClearUIData;
        if(m_ChapterClearUIData == null)
        {
            Logger.LogError("ChapterClearUIData is invalid");
            return;
        }

        var chapterData = DataTableManager.Instance.GetChapterData(m_ChapterClearUIData.chapter);
        if(chapterData == null)
        {
            Logger.LogError($"ChapterData is invalid. Chapter:{m_ChapterClearUIData.chapter}");
            return;
        }

        Rewards.SetActive(m_ChapterClearUIData.earnReward);
        if(m_ChapterClearUIData.earnReward)
        {
            GemRewardAmountTxt.text = chapterData.ChapterRewardGem.ToString("N0");
            GoldRewardAmountTxt.text = chapterData.ChapterRewardGold.ToString("N0");
            
            var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
            if(userGoodsData == null)
            {
                Logger.LogError("UserGoodsData does not exist.");
                return;
            }

            userGoodsData.Gold += chapterData.ChapterRewardGold;
            userGoodsData.Gem += chapterData.ChapterRewardGem;
            userGoodsData.SaveData();

            var goldUpdateMsg = new GoldUpdateMsg();
            goldUpdateMsg.isAdd = true;
            Messenger.Default.Publish(goldUpdateMsg);

            var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
            if(userAchievementData != null)
            {
                userAchievementData.ProgressAchievement(AchievementType.CollectGold, chapterData.ChapterRewardGold);
            }

            var gemUpdateMsg = new GemUpdateMsg();
            gemUpdateMsg.isAdd = true;
            Messenger.Default.Publish(gemUpdateMsg);
        }

        HomeBtn.GetComponent<RectTransform>().localPosition = new Vector3(0f, m_ChapterClearUIData.earnReward ? -250f : 50f, 0f);

        for (int i = 0; i < ClearFX.Length; i++)
        {
            ClearFX[i].Play();
        }
    }

    public void OnClickHomeBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        SceneLoader.Instance.LoadScene(SceneType.Lobby);
        CloseUI();
    }
}
