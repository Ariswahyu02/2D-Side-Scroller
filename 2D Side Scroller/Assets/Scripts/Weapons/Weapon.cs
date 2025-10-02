using UnityEngine;
public class Weapon : MonoBehaviour
{
    public Sprite weaponIcon;
    [SerializeField] protected int damage = 10; // Default damage value
    [SerializeField] private int baseDamage = 10;
    [SerializeField] protected float fireRate = 1.0f; // Default fire rate
    [SerializeField] private float baseFireRate = 1.0f;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float bulletSpeed = 20f;
    public int weaponTypeSlot; // 0 for equipped, 1 for inventory
    protected float lastFireTime;
    public Vector2 directionShot;

    protected Animator animator;
    protected float animationLength;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Attack()
    {
        // Default attack behavior (can be overridden)
    }

    public bool CanFire()
    {
        return Time.time >= lastFireTime + 1f / fireRate;
    }

    protected float GetAnimationSpeed()
    {
        return animationLength / (1f / fireRate);
    }

    protected float GetAnimationLengthByName(string animationName)
    {
        if (animator == null) return 0f;

        RuntimeAnimatorController rac = animator.runtimeAnimatorController;
        foreach (var clip in rac.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0f; // Animation not found
    }

    public virtual void ShootTheBullet()
    {
        // Implement bullet shooting logic
    }

    public void StopAnimation()
    {
        if (animator != null)
        {
            animator.Play("Idle");
            animator.speed = 1f; // Reset speed to normal
        }
    }

    public void ApplyBuffs()
    {
        float buffedDamage = baseDamage;
        float buffedFireRate = baseFireRate;
        foreach (var buff in InventoryManager.Instance.currentBuffs)
        {
            if (buff == null) continue;
            switch (buff.buffType)
            {
                case WeaponBuffType.PowerUp:
                    buffedDamage += buff.value;
                    break;
                case WeaponBuffType.FireRateUp:
                    buffedFireRate += buff.value;
                    break;
            }
        }
        damage = Mathf.RoundToInt(buffedDamage);
        fireRate = buffedFireRate;
    }
}
