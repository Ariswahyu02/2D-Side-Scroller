using UnityEngine;

public enum WeaponBuffType
{
    PowerUp,
    FireRateUp,
    None
}

[System.Serializable]
public class WeaponBuff : MonoBehaviour
{
    public WeaponBuffType buffType;
    public Sprite buffIcon;
    public float value;
    public int buffSlotIndex;
    public int buffSlotType; // 0 for equipped, 1 for inventory

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && !player.IsDead())
            {
                InventoryManager.Instance.AddBuffToInventory(this);
                gameObject.SetActive(false);
            }
        }
    }
    
}
