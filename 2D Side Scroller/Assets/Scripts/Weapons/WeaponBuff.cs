using UnityEngine;

public enum WeaponBuffType
{
    PowerUp,
    FireRateUp
}

[System.Serializable]
public class WeaponBuff : MonoBehaviour
{
    public WeaponBuffType buffType;
    public Sprite buffIcon;
    public float value;
    public int buffSlotIndex;
    public int buffSlotType; // 0 for equipped, 1 for inventory
    
}
