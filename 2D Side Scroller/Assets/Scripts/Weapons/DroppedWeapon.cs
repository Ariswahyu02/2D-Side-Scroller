using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    public Weapon weapon;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && !player.IsDead() && !string.IsNullOrEmpty(weapon.weaponID))
            {
                InventoryManager.Instance.PickUpWeapon(weapon.weaponID);
                gameObject.SetActive(false);
            }
        }
    }
}
