using SuperMaxim.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldUpdateMsg
{
    public bool isAdd;
}

public class GemUpdateMsg
{
    public bool isAdd;
}

public class GoodsUI : MonoBehaviour
{
    public Image GoldIcon;
    public TextMeshProUGUI GoldAmountTxt;

    public Image GemIcon;
    public TextMeshProUGUI GemAmountTxt;

    private Coroutine m_GoldIncreaseCo;
    private Coroutine m_GemIncreaseCo;
    private const float GOODS_INCRASE_DURATION = 0.5f;

    private void OnEnable()
    {
        Messenger.Default.Subscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Subscribe<GemUpdateMsg>(OnUpdateGem);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Unsubscribe<GemUpdateMsg>(OnUpdateGem);
    }

    public void SetValues()
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError("No user goods data");
        }
        else
        {
            GoldAmountTxt.text = userGoodsData.Gold.ToString("N0");
            GemAmountTxt.text = userGoodsData.Gem.ToString("N0");
        }
    }

    private void OnUpdateGold(GoldUpdateMsg goldUpdateMsg)
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            return;
        }

        AudioManager.Instance.PlaySFX(SFX.ui_get);

        if(goldUpdateMsg.isAdd)
        {
            if(m_GoldIncreaseCo != null)
            {
                StopCoroutine(m_GoldIncreaseCo);
            }

            m_GoldIncreaseCo = StartCoroutine(IncreaseGoldCo());
        }
        else
        {
            GoldAmountTxt.text = userGoodsData.Gold.ToString("N0");
        }
    }

    private IEnumerator IncreaseGoldCo()
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            yield break;
        }

        var amount = 10;
        for (int i = 0; i < amount; i++)
        {
            var goldObj = Instantiate(Resources.Load("UI/GoldMove", typeof(GameObject))) as GameObject;
            goldObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            goldObj.transform.localScale = Vector3.one;
            goldObj.transform.localPosition = Vector3.zero;
            goldObj.GetComponent<GoodsMove>().SetMove(i, GoldIcon.transform.position);
        }

        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySFX(SFX.ui_increase);

        var elapsedTime = 0f;
        var currTextValue = Convert.ToInt64(GoldAmountTxt.text.Replace(",", ""));
        var destValue = userGoodsData.Gold;
        while(elapsedTime < GOODS_INCRASE_DURATION)
        {
            var currValue = Mathf.Lerp(currTextValue, destValue, elapsedTime / GOODS_INCRASE_DURATION);
            GoldAmountTxt.text = currValue.ToString("N0");
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GoldAmountTxt.text = destValue.ToString("N0");
    }

    private void OnUpdateGem(GemUpdateMsg gemUpdateMsg)
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            return;
        }

        AudioManager.Instance.PlaySFX(SFX.ui_get);

        if(gemUpdateMsg.isAdd)
        {
            if(m_GemIncreaseCo != null)
            {
                StopCoroutine(m_GemIncreaseCo);
            }

            m_GemIncreaseCo = StartCoroutine(IncreaseGemCo());
        }
		else
        {
            GemAmountTxt.text = userGoodsData.Gem.ToString("N0");
        }
    }

    private IEnumerator IncreaseGemCo()
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            yield break;
        }

        var amount = 10;
        for (int i = 0; i < amount; i++)
        {
            var gemObj = Instantiate(Resources.Load("UI/GemMove", typeof(GameObject))) as GameObject;
            gemObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            gemObj.transform.localScale = Vector3.one;
            gemObj.transform.localPosition = Vector3.zero;
            gemObj.GetComponent<GoodsMove>().SetMove(i, GemIcon.transform.position);
        }

        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySFX(SFX.ui_increase);

        var elapsedTime = 0f;
        var currTxtValue = Convert.ToInt64(GemAmountTxt.text.Replace(",", ""));
        var destValue = userGoodsData.Gem;
        while(elapsedTime < GOODS_INCRASE_DURATION)
        {
            var currValue = Mathf.Lerp(currTxtValue, destValue, elapsedTime / GOODS_INCRASE_DURATION);
            GemAmountTxt.text = currValue.ToString("N0");
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GemAmountTxt.text = destValue.ToString("N0");
    }
}
