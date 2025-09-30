using UnityEngine;

public class AssaultRifle : Weapon
{
    public override void Attack()
    {
        // Implement assault rifle attack logic (automatic fire)
        if(!CanFire()) return;
        lastFireTime = Time.time;
        
        Debug.Log($"Assault Rifle fired! Damage: {damage}");
    }
}
