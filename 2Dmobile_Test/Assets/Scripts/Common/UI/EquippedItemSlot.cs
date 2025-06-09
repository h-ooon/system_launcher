using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemSlot : MonoBehaviour
{
    public Image AddIcon;
    public Image EquippedItemGradeBg;
    public Image EquippedItemIcon;

    private UserItemData m_EquippedItemData;

    public void SetItem(UserItemData userItemData)
    {
        m_EquippedItemData = userItemData;

        AddIcon.gameObject.SetActive(false);
        EquippedItemGradeBg.gameObject.SetActive(true);
        EquippedItemIcon.gameObject.SetActive(true);

        var itemGrade = (ItemGrade)((m_EquippedItemData.ItemId / 1000) % 10);
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");
        if(gradeBgTexture != null)
        {
            EquippedItemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }

        StringBuilder sb = new StringBuilder(m_EquippedItemData.ItemId.ToString());
        sb[1] = '1';
        var itemIconName = sb.ToString();

        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        if(itemIconTexture != null)
        {
            EquippedItemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }
    }

    public void ClearItem()
    {
        m_EquippedItemData = null;

        AddIcon.gameObject.SetActive(true);
        EquippedItemGradeBg.gameObject.SetActive(false);
        EquippedItemIcon.gameObject.SetActive(false);
    }

    public void OnClickEquippedItemSlot()
    {
        var uiData = new EquipmentUIData();
        uiData.SerialNumber = m_EquippedItemData.SerialNumber;
        uiData.ItemId = m_EquippedItemData.ItemId;
        uiData.IsEquipped = true;
        UIManager.Instance.OpenUI<EquipmentUI>(uiData);
    }
}