using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public Weapon currentWeapon;
    public Weapon[] availableWeapons;
    public WeaponBuff[] currentBuffs;
    public WeaponBuff[] availableBuffs;

    protected override void Awake()
    {
        base.Awake();
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
        // Remove the specific buff instance from availableBuffs
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

    public void EquipNewWeapon(Weapon weapon, WeaponUI weaponUI)
    {
        currentWeapon?.StopAnimation();
        weaponUI.SetWeaponUI(null);
        // Remove the specific weapon instance from availableWeapons
        for (int i = 0; i < availableWeapons.Length; i++)
        {
            if (availableWeapons[i] == weapon)
            {
                availableWeapons[i] = null;
                break;
            }
        }
        SendBackEquippedWeaponToInventory(currentWeapon);

        currentWeapon = weapon;
        currentWeapon.weaponTypeSlot = 0; // 0 for equipped
        currentWeapon.ApplyBuffs();
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
}
