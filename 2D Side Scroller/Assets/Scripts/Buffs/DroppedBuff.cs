using UnityEngine;

public class DroppedBuff : MonoBehaviour
{
    [SerializeField] private WeaponBuff weaponBuff;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager.Instance.AddBuffToInventory(weaponBuff);
            gameObject.SetActive(false);
            SoundManager.Instance.PlaySFX("Buff");
        }
    }
}
