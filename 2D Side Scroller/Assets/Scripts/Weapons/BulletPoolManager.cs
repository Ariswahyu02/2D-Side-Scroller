using UnityEngine;
using System.Collections.Generic;

public class BulletPoolManager : Singleton<BulletPoolManager>
{
    [System.Serializable]
    public class BulletPool
    {
        public WeaponType weaponType;
        public Bullet bulletPrefab;
        public int poolSize = 20;
        [HideInInspector] public Queue<Bullet> pool = new Queue<Bullet>();
    }

    [SerializeField] private BulletPool[] bulletPools;

    protected override void Awake()
    {
        base.Awake();
        foreach (var bp in bulletPools)
        {
            for (int i = 0; i < bp.poolSize; i++)
            {
                Bullet bullet = Instantiate(bp.bulletPrefab, transform);
                bullet.gameObject.SetActive(false);
                bp.pool.Enqueue(bullet);
            }
        }
    }

    public Bullet GetBullet(WeaponType weaponType, Vector2 position, Vector2 direction, float speed, int damage)
    {
        var bp = System.Array.Find(bulletPools, p => p.weaponType == weaponType);
        if (bp == null)
        {
            Debug.LogError($"Bullet pool for weapon '{weaponType}' not found!");
            return null;
        }
        Bullet bullet = bp.pool.Count > 0 ? bp.pool.Dequeue() : Instantiate(bp.bulletPrefab, transform);
        bullet.transform.position = position;
        bullet.Init(direction, speed, damage);
        bp.pool.Enqueue(bullet);
        return bullet;
    }
}
