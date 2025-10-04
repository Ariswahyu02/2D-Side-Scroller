using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("Weapon References")]
    public Weapon[] weaponInPlayerHand;

    [Header("Player Inventory")]
    public Weapon currentWeapon;
    public Weapon[] availableWeapons;
    public WeaponBuff[] currentBuffs;
    public WeaponBuff[] availableBuffs;

    private readonly Dictionary<string, Weapon> handMap = new Dictionary<string, Weapon>();

    protected override void Awake()
    {
        base.Awake();
        handMap.Clear();

        // Add weapon and id to dict
        foreach (var weapon in weaponInPlayerHand)
        {
            if (weapon != null && !string.IsNullOrEmpty(weapon.weaponID) && !handMap.ContainsKey(weapon.weaponID))
            {
                handMap.Add(weapon.weaponID, weapon);
            }
        }

        VerifyWeaponInPlayerHand(currentWeapon);
    }

    public float GetAppliedDamageBuffValue()
    {
        float totalBuff = 0f;
        foreach (var buff in currentBuffs)
        {
            if (buff != null && buff.buffType == WeaponBuffType.PowerUp)
            {
                totalBuff += buff.value;
            }
        }
        return totalBuff;
    }

    public float GetAppliedFireRateBuffValue()
    {
        float totalBuff = 0f;
        foreach (var buff in currentBuffs)
        {
            if (buff != null && buff.buffType == WeaponBuffType.FireRateUp)
            {
                totalBuff += buff.value;
            }
        }
        return totalBuff;
    }

    public void EquipBuff(WeaponBuff buff, BuffUI buffUI)
    {
        for (int i = 0; i < currentBuffs.Length; i++)
        {
            if (currentBuffs[i] == null)
            {
                currentBuffs[i] = buff;
                currentBuffs[i].buffSlotIndex = i;
                currentBuffs[i].buffSlotType = 0; // 0 for equipped
                buffUI.SetBuffUI(null);
                break;
            }
        }
        for (int i = 0; i < availableBuffs.Length; i++)
        {
            if (availableBuffs[i] == buff)
            {
                availableBuffs[i] = null;
                break;
            }
        }
        currentWeapon.ApplyBuffs();
    }

    public void EquipNewWeapon(string weaponID, WeaponUI weaponUI)
    {
        if (string.IsNullOrEmpty(weaponID)) return;

        Weapon target = null;
        for (int i = 0; i < availableWeapons.Length; i++)
        {
            var w = availableWeapons[i];
            if (w != null && w.weaponID == weaponID)
            {
                target = w;
                availableWeapons[i] = null;
                break;
            }
        }

        if (target == null) return;

        currentWeapon?.StopAnimation();
        weaponUI.SetWeaponUI(null);

        SendBackEquippedWeaponToInventory(currentWeapon);

        currentWeapon = target;
        currentWeapon.weaponTypeSlot = 0;
        currentWeapon.ApplyBuffs();

        GameUI.Instance.UpdateCurrentWeaponAndBuffUI();
        VerifyWeaponInPlayerHand(currentWeapon);
    }

    public void UnEquipBuff(int indexClickedBuff, BuffUI buffUI)
    {
        for (int i = 0; i < currentBuffs.Length; i++)
        {
            if (currentBuffs[i] != null && currentBuffs[i].buffSlotIndex == indexClickedBuff)
            {
                WeaponBuff buffToReturn = currentBuffs[i];
                buffToReturn.buffSlotType = 1; // 1 for inventory
                currentBuffs[i] = null;
                buffUI.SetBuffUI(null);
                SendBackEquippedBuffsToInventory(buffToReturn);
            }
        }

        currentWeapon.ApplyBuffs();
    }

    public void SendBackEquippedBuffsToInventory(WeaponBuff buff)
    {
        for (int i = 0; i < availableBuffs.Length; i++)
        {
            if (availableBuffs[i] == null)
            {
                availableBuffs[i] = buff;
                break;
            }
        }
    }

    public void SendBackEquippedWeaponToInventory(Weapon weapon)
    {
        for (int i = 0; i < availableWeapons.Length; i++)
        {
            if (availableWeapons[i] == null)
            {
                availableWeapons[i] = weapon;
                weapon.weaponTypeSlot = 1; // 1 for inventory
                break;
            }
        }
    }

    public bool CanEquipBuff()
    {
        for (int i = 0; i < currentBuffs.Length; i++)
        {
            if (currentBuffs[i] == null)
            {
                return true;
            }
        }
        return false;
    }

    public void AddBuffToInventory(WeaponBuff buff)
    {
        for (int i = 0; i < availableBuffs.Length; i++)
        {
            if (availableBuffs[i] == null)
            {
                availableBuffs[i] = buff;
                buff.buffSlotType = 1; // 1 for inventory
                break;
            }
        }
    }

    public void PickUpWeapon(string weaponID)
    {
        if (string.IsNullOrEmpty(weaponID)) return;

        if (!handMap.TryGetValue(weaponID, out var handWeapon) || handWeapon == null)
        {
            Debug.LogWarning($"Weapon with ID: '{weaponID}' Not Found");
            return;
        }

        for (int i = 0; i < availableWeapons.Length; i++)
        {
            if (availableWeapons[i] == handWeapon)
                return;

            if (availableWeapons[i] == null)
            {
                availableWeapons[i] = handWeapon;
                handWeapon.weaponTypeSlot = 1; // 1 = inventory
                break;
            }
        }

        GameUI.Instance.UpdateInventoryUI();
    }
    
    public void VerifyWeaponInPlayerHand(Weapon weapon)
    {
        for (int i = 0; i < weaponInPlayerHand.Length; i++)
        {
            weaponInPlayerHand[i].gameObject.SetActive(false);

            if (weaponInPlayerHand[i].weaponID == weapon.weaponID)
            {
                weaponInPlayerHand[i].gameObject.SetActive(true);
            }
        }
    }
}
