using UnityEngine;

public class Glock : Weapon
{
    protected override void Awake()
    {
        base.Awake();
        animationLength = GetAnimationLengthByName("Shoot");
        Debug.Log("animation length: " + animationLength);
    }

    public override void Attack()
    {   
        if (!CanFire()) return;
        lastFireTime = Time.time;

        // Spawn bullet using singleton
        if (firePoint != null)
        {
            if (animator != null)
            {
                Debug.Log("animation length: " + animationLength);
                Debug.Log("animation speed: " + GetAnimationSpeed());
                animator.speed = GetAnimationSpeed();
                animator.Play("Shoot");
            }
            Debug.Log($"Pistol fired! Damage: {damage}");

            BeginShootCycle();
        }

    }

    public override void ShootTheBullet()
    {
        shotFiredThisCycle = true;
        // Implement bullet shooting logic
        if (BulletPoolManager.Instance != null && firePoint != null)
        {
            BulletPoolManager.Instance.GetBullet(
                WeaponType.Glock,
                firePoint.position,
                directionShot,
                bulletSpeed,
                damage
            );

            SoundManager.Instance.PlaySFX("Glock");
        }
    }
}
