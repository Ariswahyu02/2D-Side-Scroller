using UnityEngine;

public class Zombie : Enemy
{
    protected override void Update()
    {
        // Example: slow movement
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    protected override void Die()
    {
        // Custom death logic for zombie
        Debug.Log("Zombie died!");
        base.Die();
    }
}
