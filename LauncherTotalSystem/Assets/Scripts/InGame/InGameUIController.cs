using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    public void Init()
    {

    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            if (!InGameManager.Instance.IsPaused && !InGameManager.Instance.IsStageCleared)
            {
                var uiData = new BaseUIData();
                UIManager.Instance.OpenUI<PauseUI>(uiData);

                InGameManager.Instance.PauseGame();
            }
        }
    }

    private void Update()
    {
        if(!InGameManager.Instance.IsPaused && !InGameManager.Instance.IsStageCleared)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            var uiData = new BaseUIData();
            UIManager.Instance.OpenUI<PauseUI>(uiData);

            InGameManager.Instance.PauseGame();
        }
    }

    public void OnClickPauseBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<PauseUI>(uiData);

        InGameManager.Instance.PauseGame();
    }
}
