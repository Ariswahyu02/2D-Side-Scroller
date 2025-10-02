using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image buffIcon;
    [SerializeField] private Text buffDescription;
    [SerializeField] private WeaponBuff buff;

    public void SetBuffUI(WeaponBuff buff)
    {
        if (buff != null)
        {
            buffIcon.sprite = buff.buffIcon;
            this.buff = buff;
        }
        else
        {
            buffIcon.sprite = null;
            this.buff = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (buff == null) return;

        if (buff.buffSlotType == 0) // Equipped
        {
            InventoryManager.Instance.UnEquipBuff(buff.buffSlotIndex, this);
        }
        else if (buff.buffSlotType == 1) // In Inventory
        {
            InventoryManager.Instance.EquipBuff(buff, this);
        }

        GameUI.Instance.UpdateCurrentWeaponAndBuffUI();
        GameUI.Instance.UpdateInventoryUI();
    }
}
