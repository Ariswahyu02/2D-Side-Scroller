using UnityEngine;
using System.Collections.Generic;

public class DroppedItemPoolManager : Singleton<DroppedItemPoolManager>
{
    [System.Serializable]
    public class BuffPool
    {
        public WeaponBuffType buffType;
        public WeaponBuff buffPrefab;
        public int poolSize = 10;
        [HideInInspector] public Queue<WeaponBuff> pool = new Queue<WeaponBuff>();
    }

    [Header("BUFF Pools")]
    [SerializeField] private BuffPool[] buffPools;

    [System.Serializable]
    public class WeaponPickupPool
    {
        public WeaponType weaponType;
        public DroppedWeapon pickupPrefab;
        public int poolSize = 6;
        [HideInInspector] public Queue<DroppedWeapon> pool = new Queue<DroppedWeapon>();
    }

    [Header("WEAPON Pickup Pools")]
    [SerializeField] private WeaponPickupPool[] weaponPickupPools;

    protected override void Awake()
    {
        base.Awake();

        if (buffPools != null)
        {
            foreach (var bp in buffPools)
            {
                if (bp.buffPrefab == null) continue;
                int size = Mathf.Max(1, bp.poolSize);
                for (int i = 0; i < size; i++)
                {
                    var obj = Instantiate(bp.buffPrefab, transform);
                    obj.gameObject.SetActive(false);
                    bp.pool.Enqueue(obj);
                }
            }
        }

        if (weaponPickupPools != null)
        {
            foreach (var wp in weaponPickupPools)
            {
                if (wp.pickupPrefab == null) continue;
                int size = Mathf.Max(1, wp.poolSize);
                for (int i = 0; i < size; i++)
                {
                    var go = Instantiate(wp.pickupPrefab, transform);
                    go.gameObject.SetActive(false);
                    wp.pool.Enqueue(go);
                }
            }
        }
    }

    public WeaponBuff GetBuff(WeaponBuffType type, Vector2 position, float rotationZ = 0f)
    {
        var bp = System.Array.Find(buffPools, p => p.buffType == type);
        if (bp == null) return null;

        if (bp.pool.Count == 0)
        {
            var extra = Instantiate(bp.buffPrefab, transform);
            extra.gameObject.SetActive(false);
            bp.pool.Enqueue(extra);
        }

        var buff = bp.pool.Dequeue();
        buff.transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, rotationZ));
        buff.gameObject.SetActive(true);
        bp.pool.Enqueue(buff);
        return buff;
    }

    public DroppedWeapon GetWeaponPickup(WeaponType type, Vector2 position, float rotationZ = 0f)
    {
        var wp = System.Array.Find(weaponPickupPools, p => p.weaponType == type);
        if (wp == null) return null;

        if (wp.pool.Count == 0)
        {
            var extra = Instantiate(wp.pickupPrefab, transform);
            extra.gameObject.SetActive(false);
            wp.pool.Enqueue(extra);
        }

        var weapon = wp.pool.Dequeue();
        weapon.transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, rotationZ));
        weapon.gameObject.SetActive(true);
        wp.pool.Enqueue(weapon);
        return weapon;
    }
}
