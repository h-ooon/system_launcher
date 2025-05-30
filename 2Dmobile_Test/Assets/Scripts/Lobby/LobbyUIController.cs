using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LobbyUIController : MonoBehaviour
{

    // public Camera mainCamera;
    // public Camera myCamera;

    // public void Start()
    // {
    //     mainCamera = Camera.main;
    //     myCamera = GetComponentInChildren<Camera>();

    //     var mainCameraData = mainCamera.GetUniversalAdditionalCameraData();

    //     if (!mainCameraData.cameraStack.Contains(myCamera))
    //     {
    //         mainCameraData.cameraStack.Insert(0, myCamera);
    //     }
    // }

    public void Init()
    {

    }

    public void OnClickSettingsBtn()
    {
        Logger.Log($"{GetType()}::OnClickSettingsBtn");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<SettingsUI>(uiData);
    }
}
