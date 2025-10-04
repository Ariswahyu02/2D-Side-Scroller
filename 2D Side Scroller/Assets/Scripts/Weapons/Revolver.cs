using UnityEngine;

public class Revolver : Weapon
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

        if (BulletPoolManager.Instance != null && firePoint != null)
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
        // Implement bullet shooting logic
        if (BulletPoolManager.Instance != null && firePoint != null)
        {
            BulletPoolManager.Instance.GetBullet(
                WeaponType.Revolver,
                firePoint.position,
                directionShot,
                bulletSpeed,
                damage
            );

            SoundManager.Instance.PlaySFX("Revolver");
        }
    }
}
