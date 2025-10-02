using UnityEngine;

public class Robot : Enemy
{
    protected override void Update()
    {
        // Example: faster movement
        // transform.position += Vector3.left * (moveSpeed * 2f) * Time.deltaTime;
    }

    protected override void Die()
    {
        // Custom death logic for robot
        Debug.Log("Robot exploded!");
        base.Die();
    }
}
