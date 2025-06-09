using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    public TextMeshProUGUI CurrChapterNameTxt;
    public RawImage CurrChapterBg;

    public void Init()
    {
        UIManager.Instance.EnableGoodsUI(true);
        SetCurrChapter();
    }

    public void SetCurrChapter()
    {
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        var currChapterData = DataTableManager.Instance.GetChapterData(userPlayData.SelectedChapter);
        if(currChapterData == null)
        {
            Logger.LogError("CurrChapterData does not exist.");
            return;
        }

        CurrChapterNameTxt.text = currChapterData.ChapterName;
        var bgTexture = Resources.Load($"ChapterBG/Background_{userPlayData.SelectedChapter.ToString("D3")}") as Texture2D;
        if(bgTexture != null)
        {
            CurrChapterBg.texture = bgTexture;
        }
    }

    public void OnClickSettingsBtn()
    {
        Logger.Log($"{GetType()}::OnClickSettingsBtn");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<SettingsUI>(uiData);
    }

    public void OnClickProfileBtn()
    {
        Logger.Log($"{GetType()}::OnClickProfileBtn");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<InventoryUI>(uiData);
    }

    public void OnClickCurrChapter()
    {
        Logger.Log($"{GetType()}::OnClickCurrChapter");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<ChapterListUI>(uiData);
    }

    public void OnClickStartBtn()
    {
        Logger.Log($"{GetType()}::OnClickStartBtn");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        AudioManager.Instance.StopBGM();

        LobbyManager.Instance.StartInGame();
    }

    public void OnClickAchievementBtn()
    {
        Logger.Log($"{GetType()}::OnClickAchievementBtn");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<AchievementUI>(uiData);
    }
}
