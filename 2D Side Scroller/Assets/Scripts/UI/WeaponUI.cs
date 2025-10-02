using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite weaponIcon;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Weapon weapon;

    public void SetWeaponUI(Weapon weapon)
    {
        if (weapon != null)
        {
            weaponImage.sprite = weapon.weaponIcon;
            this.weapon = weapon;
        }
        else
        {
            weaponImage.sprite = null;
            this.weapon = null;
            weaponImage.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (weapon == null) return;

        if (weapon.weaponTypeSlot == 1) // In Inventory
        {
            InventoryManager.Instance.EquipNewWeapon(weapon, this);
        }

        GameUI.Instance.UpdateCurrentWeaponAndBuffUI();
        GameUI.Instance.UpdateInventoryUI();
    }
}
