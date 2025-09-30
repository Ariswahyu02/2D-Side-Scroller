using UnityEngine;

public class Shotgun : Weapon
{
    public int Pellets => 8;
    public float SpreadAngle => 15f; // degrees

    public override void Attack()
    {
        // Implement shotgun attack logic (spread shot)
        if(!CanFire()) return;
        lastFireTime = Time.time;

        Debug.Log($"Shotgun fired! Damage: {damage}");
    }
}
