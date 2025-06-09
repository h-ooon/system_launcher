using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class CustomTools : Editor
{
    [MenuItem("Inflearn/Add User Gem(+100)")]
    public static void AddUserGem()
    {
        var gem = long.Parse(PlayerPrefs.GetString("Gem"));
        gem += 100;

        PlayerPrefs.SetString("Gem", gem.ToString());
        PlayerPrefs.Save();
    }

    [MenuItem("Inflearn/Add User Gold(+100)")]
    public static void AddUserGold()
    {
        var gold = long.Parse(PlayerPrefs.GetString("Gold"));
        gold += 100;

        PlayerPrefs.SetString("Gold", gold.ToString());
        PlayerPrefs.Save();
    }
}
#endif 