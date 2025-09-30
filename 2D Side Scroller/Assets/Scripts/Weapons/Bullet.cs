using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private int damage;
    private Vector2 direction;

    public void Init(Vector2 dir, float spd, int dmg)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Implement damage logic here
        // Example: if (other.CompareTag("Enemy")) { /* deal damage */ }
        if(other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
