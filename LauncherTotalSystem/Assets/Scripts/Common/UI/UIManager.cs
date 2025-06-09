using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager>
{
    public Transform UICanvasTrs;
    public Transform ClosedUITrs;

    public Image FadeImg;

    private BaseUI m_FrontUI;
    private Dictionary<System.Type, GameObject> m_OpenUIPool = new Dictionary<System.Type, GameObject>();
    private Dictionary<System.Type, GameObject> m_ClosedUIPool = new Dictionary<System.Type,GameObject>();

    private GoodsUI m_GoodsUI;

    protected override void Init()
    {
        base.Init();

        FadeImg.transform.localScale = Vector3.zero;

        m_GoodsUI = FindObjectOfType<GoodsUI>();
        if(!m_GoodsUI)
        {
            Logger.LogError("No goods ui component found.");
        }
    }

    private BaseUI GetUI<T>(out bool isAlreadyOpen)
    {
        System.Type uiType = typeof(T);

        BaseUI ui = null;
        isAlreadyOpen = false;

        if(m_OpenUIPool.ContainsKey(uiType))
        {
            ui = m_OpenUIPool[uiType].GetComponent<BaseUI>();
            isAlreadyOpen = true;
        }
        else if(m_ClosedUIPool.ContainsKey(uiType))
        {
            ui = m_ClosedUIPool[uiType].GetComponent<BaseUI>();
            m_ClosedUIPool.Remove(uiType);
        }
        else
        {
            var uiObj = Instantiate(Resources.Load($"UI/{uiType}", typeof(GameObject))) as GameObject;
            ui = uiObj.GetComponent<BaseUI>();
        }

        return ui;
    }

    public void OpenUI<T>(BaseUIData uiData)
    {
        System.Type uiType = typeof(T);

        Logger.Log($"{GetType()}::OpenUI({uiType})");

        bool isAlreadyOpen = false;
        var ui = GetUI<T>(out isAlreadyOpen);

        if(!ui)
        {
            Logger.LogError($"{uiType} does not exist.");
            return;
        }

        if(isAlreadyOpen)
        {
            Logger.LogError($"{uiType} is already open.");
            return;
        }

        var siblingIndex = UICanvasTrs.childCount - 2;
        ui.Init(UICanvasTrs);
        ui.transform.SetSiblingIndex(siblingIndex);
        ui.gameObject.SetActive(true);
        ui.SetInfo(uiData);
        ui.ShowUI();

        m_FrontUI = ui;
        m_OpenUIPool[uiType] = ui.gameObject;
    }

    public void CloseUI(BaseUI ui)
    {
        System.Type uiType = ui.GetType();

        Logger.Log($"{GetType()}::CloseUI ({uiType})");

        ui.gameObject.SetActive(false);
        m_OpenUIPool.Remove(uiType);
        m_ClosedUIPool[uiType] = ui.gameObject;
        ui.transform.SetParent(ClosedUITrs);

        m_FrontUI = null;
        var lastChild = UICanvasTrs.GetChild(UICanvasTrs.childCount - 3);
        if(lastChild)
        {
            m_FrontUI = lastChild.gameObject.GetComponent<BaseUI>();
        }
    }

    public BaseUI GetActiveUI<T>()
    {
        var uiType = typeof(T);
        return m_OpenUIPool.ContainsKey(uiType) ? m_OpenUIPool[uiType].GetComponent<BaseUI>() : null;
    }

    public bool ExistsOpenUI()
    {
        return m_FrontUI != null;
    }

    public BaseUI GetCurrentFrontUI()
    {
        return m_FrontUI;
    }

    public void CloseCurrentFrontUI()
    {
        m_FrontUI.CloseUI();
    }

    public void CloseAllOpenUI()
    {
        while(m_FrontUI)
        {
            m_FrontUI.CloseUI(true);
        }
    }

    public void EnableGoodsUI(bool value)
    {
        m_GoodsUI.gameObject.SetActive(value);

        if(value)
        {
            m_GoodsUI.SetValues();
        }
    }

    public void Fade(Color color, float startAlpha, float endAlpha, float duration, float startDelay, bool deactivateOnFinish, Action onFinish = null)
    {
        StartCoroutine(FadeCo(color, startAlpha, endAlpha, duration, startDelay, deactivateOnFinish, onFinish));
    }

    private IEnumerator FadeCo(Color color, float startAlpha, float endAlpha, float duration, float startDelay, bool deactivateOnFinish, Action onFinish)
    {
        yield return new WaitForSeconds(startDelay);

        FadeImg.transform.localScale = Vector3.one;
        FadeImg.color = new Color(color.r, color.g, color.b, startAlpha);

        var startTime = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup - startTime < duration)
        {
            FadeImg.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startAlpha, endAlpha, (Time.realtimeSinceStartup - startTime) / duration));
            yield return null;
        }

        FadeImg.color = new Color(color.r, color.g, color.b, endAlpha);

        if(deactivateOnFinish)
        {
            FadeImg.transform.localScale = Vector3.zero;
        }

        onFinish?.Invoke();
    }
}
