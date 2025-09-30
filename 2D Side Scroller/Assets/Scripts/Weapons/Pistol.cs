using UnityEngine;

public class Pistol : Weapon
{
    protected override void Awake()
    {
        base.Awake();
        animationLength = GetAnimationLengthByName("Shoot");
        Debug.Log("animation length: " + animationLength);
    }

    public override void Attack()
    {
        // Implement pistol attack logic (single shot)
        if (!CanFire()) return;
        lastFireTime = Time.time;

        // Spawn bullet using singleton
        if (BulletPoolManager.Instance != null && firePoint != null)
        {
            // BulletPoolManager.Instance.GetBullet(
            //     WeaponType.Pistol,
            //     firePoint.position,
            //     firePoint.right, // or firePoint.up depending on your setup
            //     bulletSpeed,
            //     damage
            // );

            if (animator != null)
            {
                Debug.Log("animation length: " + animationLength);
                Debug.Log("animation speed: " + GetAnimationSpeed());
                animator.speed = GetAnimationSpeed();
                animator.Play("Shoot");
            }
            Debug.Log($"Pistol fired! Damage: {damage}");
        }

    }

    public override void ShootTheBullet()
    {
        // Implement bullet shooting logic
        if (BulletPoolManager.Instance != null && firePoint != null)
        {
            BulletPoolManager.Instance.GetBullet(
                WeaponType.Pistol,
                firePoint.position,
                firePoint.right, // or firePoint.up depending on your setup
                bulletSpeed,
                damage
            );
        }
    }
}
