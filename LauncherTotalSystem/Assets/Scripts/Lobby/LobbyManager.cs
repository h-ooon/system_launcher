using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public LobbyUIController LobbyUIController { get; private set; }

    private bool m_IsLoadingInGame;

    protected override void Init()
    {
        m_IsDestroyOnLoad = true;
        m_IsLoadingInGame = false;
        
        base.Init();
    }

    private void Start()
    {
        LobbyUIController = FindObjectOfType<LobbyUIController>();
        if(!LobbyUIController)
        {
            Logger.Log("LobbyUIController does not exist.");
            return;
        }

        LobbyUIController.Init();
        AudioManager.Instance.PlayBGM(BGM.lobby);
    }

    public void StartInGame()
    {
        if(m_IsLoadingInGame)
        {
            return;
        }

        m_IsLoadingInGame = true;

        UIManager.Instance.Fade(Color.black, 0f, 1f, 0.5f, 0f, false, () =>
        {
            UIManager.Instance.CloseAllOpenUI();
            SceneLoader.Instance.LoadScene(SceneType.InGame);
        });
    }
}
