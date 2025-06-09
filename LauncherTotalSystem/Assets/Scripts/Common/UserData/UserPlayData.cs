using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayData : IUserData
{
    public int MaxClearedChapter { get; set; }
    //not saved to playerprefs
    public int SelectedChapter { get; set; } = 1;

    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");

        MaxClearedChapter = 0;
        SelectedChapter = 1;
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;

        try
        {
            MaxClearedChapter = PlayerPrefs.GetInt("MaxClearedChapter");
            SelectedChapter = MaxClearedChapter + 1;

            result = true;

            Logger.Log($"MaxClearedChpater:{MaxClearedChapter}");
        }
        catch (System.Exception e)
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
            PlayerPrefs.SetInt("MaxClearedChapter", MaxClearedChapter);
            result = true;

            Logger.Log($"MaxClearedChpater:{MaxClearedChapter}");
        }
        catch (System.Exception e)
        {
            Logger.Log($"Save failed. (" + e.Message + ")");
        }

        return result;
    }
}
