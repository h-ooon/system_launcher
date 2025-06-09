using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; private set; }
    public bool IsStageCleared { get; private set; }
    public bool IsPaused { get; private set; }

    private int m_SelectedChapter;
    private ChapterData m_CurrChapterData;
    private int m_CurrStage;
    private const string STAGE_PATH = "Stages/";
    private Transform m_StageTrs;
    private SpriteRenderer m_Bg;

    private GameObject m_LoadedStage;
    //private Coin[] m_Coins;

    protected override void Init()
    {
        m_IsDestroyOnLoad = true;

        base.Init();

        // InitVariables();
        // LoadBg();
        //LoadStage();

        UIManager.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, true);
    }

    private void InitVariables()
    {
        Logger.Log($"{GetType()}::InitVariables");

        m_StageTrs = GameObject.Find("Stage").transform;
        m_Bg = GameObject.Find("Bg").GetComponent<SpriteRenderer>();
        m_CurrStage = 20;

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        m_SelectedChapter = userPlayData.SelectedChapter;

        m_CurrChapterData = DataTableManager.Instance.GetChapterData(m_SelectedChapter);
        if(m_CurrChapterData == null)
        {
            Logger.LogError($"ChapterData does not exist. Chapter:{m_SelectedChapter}");
            return;
        }
    }

    private void LoadBg()
    {
        var bgTexture = Resources.Load($"ChapterBG/Background_{m_SelectedChapter.ToString("D3")}") as Texture2D;
        if (bgTexture != null)
        {
            m_Bg.sprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(0.5f, 0.5f));
        }
    }

    private void LoadStage()
    {
        Logger.Log($"{GetType()}::LoadStage");
        Logger.Log($"Chapter:{m_SelectedChapter} Stage:{m_CurrStage}");

        if(m_LoadedStage)
        {
            Destroy(m_LoadedStage);
        }

        // var stageObj = Instantiate(Resources.Load($"{STAGE_PATH}C{m_SelectedChapter}/C{m_SelectedChapter}_S{m_CurrStage}", typeof(GameObject))) as GameObject;
        // stageObj.transform.SetParent(m_StageTrs);
        // stageObj.transform.localScale = Vector3.one;
        // stageObj.transform.localPosition = Vector3.zero;
        // m_LoadedStage = stageObj;

        // m_Coins = FindObjectsOfType<Coin>();
    }

    private void Start()
    {
        InGameUIController = FindObjectOfType<InGameUIController>();
        if(!InGameUIController)
        {
            Logger.LogError("InGameUIController does not exist.");
            return;
        }

        InGameUIController.Init();

        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData != null)
        {
            userAchievementData.ProgressAchievement(AchievementType.ClearChapter1, 1);
            userAchievementData.ProgressAchievement(AchievementType.ClearChapter3, 1);
        }
    }

    private void Update()
    {
        CheckStageClear();    
    }

    private void CheckStageClear()
    {
        if(IsStageCleared)
        {
            return;
        }

        /*
        var inactiveCnt = 0;
        for (int i = 0; i < m_Coins.Length; i++)
        {
            if(!m_Coins[i].gameObject.activeSelf)
            {
                inactiveCnt++;
            }
        }

        if(inactiveCnt == m_Coins.Length)
        {
            ClearStage();
        }
        */
    }

    private void ClearStage()
    {
        Logger.Log($"{GetType()}::ClearStage");

        IsStageCleared = true;

        StartCoroutine(ShowStageClearCo());
    }

    private IEnumerator ShowStageClearCo()
    {
        AudioManager.Instance.PlaySFX(SFX.stage_clear);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<StageClearUI>(uiData);

        yield return new WaitForSeconds(1f);

        var stageClearUI = UIManager.Instance.GetActiveUI<StageClearUI>();
        if(stageClearUI)
        {
            stageClearUI.CloseUI();
        }

        if(IsAllClear())
        {
            ClearChapter();
        }
        else
        {
            IsStageCleared = false;
            m_CurrStage++;
            LoadStage();
        }
    }

    private bool IsAllClear()
    {
        return m_CurrStage == m_CurrChapterData.TotalStages;
    }

    private void ClearChapter()
    {
        AudioManager.Instance.PlaySFX(SFX.chapter_clear);

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        var uiData = new ChapterClearUIData();
        uiData.chapter = m_SelectedChapter;
        uiData.earnReward = m_SelectedChapter > userPlayData.MaxClearedChapter;
        UIManager.Instance.OpenUI<ChapterClearUI>(uiData);

        if(m_SelectedChapter > userPlayData.MaxClearedChapter)
        {
            userPlayData.MaxClearedChapter++;
            userPlayData.SelectedChapter = userPlayData.MaxClearedChapter + 1;
            userPlayData.SaveData();
        }

        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData != null)
        {
            switch (m_SelectedChapter)
            {
                case 1:
                    userAchievementData.ProgressAchievement(AchievementType.ClearChapter1, 1);
                    break;
                case 2:
                    userAchievementData.ProgressAchievement(AchievementType.ClearChapter2, 1);
                    break;
                case 3:
                    userAchievementData.ProgressAchievement(AchievementType.ClearChapter3, 1);
                    break;
                default:
                    break;
            }
        }
    }

    public void PauseGame()
    {
        IsPaused = true;

/*        GameManager.Instance.Paused = true;
        LevelManager.Instance.ToggleCharacterPause();*/
    }

    public void ResumeGame()
    {
        IsPaused = false;

/*        GameManager.Instance.Paused = false;
        LevelManager.Instance.ToggleCharacterPause();*/
    }
}
