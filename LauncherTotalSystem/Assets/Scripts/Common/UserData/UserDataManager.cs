using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataConvertHelper
{
    public static string ConvertDataToBinary<T>(T data)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();
        binaryFormatter.Serialize(memoryStream, data);
        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public static T ConvertBinaryToData<T>(string binaryData)
    {
        byte[] bytes = Convert.FromBase64String(binaryData);
        MemoryStream memoryStream = new MemoryStream(bytes);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        return (T)binaryFormatter.Deserialize(memoryStream);
    }
}

public class UserDataManager : SingletonBehaviour<UserDataManager>
{
    //저장된 유저 데이터 존재 여부
    public bool ExistsSavedData { get; private set; }
    //모든 유저 데이터 인스턴스를 저장하는 컨테이너
    public List<IUserData> UserDataList { get; private set; } = new List<IUserData>();

    protected override void Init()
    {
        base.Init();

        //모든 유저 데이터를 UserDataList에 추가
        UserDataList.Add(new UserSettingsData());
        UserDataList.Add(new UserGoodsData());
        UserDataList.Add(new UserInventoryData());
        UserDataList.Add(new UserPlayData());
        UserDataList.Add(new UserAchievementData());
    }

    public void SetDefaultUserData()
    {
        for (int i = 0; i < UserDataList.Count; i++)
        {
            UserDataList[i].SetDefaultData();
        }
    }

    public void LoadUserData()
    {
        ExistsSavedData = PlayerPrefs.GetInt("ExistsSavedData") == 1 ? true : false;

        if(ExistsSavedData)
        {
            for (int i = 0; i < UserDataList.Count; i++)
            {
                UserDataList[i].LoadData();
            }
        }
    }

    public void SaveUserData()
    {
        bool hasSaveError = false;

        for (int i = 0; i < UserDataList.Count; i++)
        {
            bool isSaveSuccess = UserDataList[i].SaveData();
            if(!isSaveSuccess)
            {
                hasSaveError = true;
            }
        }

        if(!hasSaveError)
        {
            ExistsSavedData = true;
            PlayerPrefs.SetInt("ExistsSavedData", 1);
            PlayerPrefs.Save();
        }
    }

    public T GetUserData<T>() where T : class, IUserData
    {
        return UserDataList.OfType<T>().FirstOrDefault();
    }
}
