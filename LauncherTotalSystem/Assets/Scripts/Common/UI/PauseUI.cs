using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : BaseUI
{
    public void OnClickResume()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        InGameManager.Instance.ResumeGame();

        CloseUI();
    }

    public void OnClickHome()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        SceneLoader.Instance.LoadScene(SceneType.Lobby);

        CloseUI();
    }
}
