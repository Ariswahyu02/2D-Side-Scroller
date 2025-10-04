using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed;
    public int damage;

    private Vector2 direction;

    public virtual void Init(Vector2 dir, float spd, int dmg)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
        gameObject.SetActive(true);
    }

    protected virtual void Update()
    {
        // Gerakan manual
        transform.Translate(direction * speed * Time.deltaTime);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        OnImpact(collision.collider);
    }

    protected virtual void OnImpact(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Deactivate();
            }
        }
    }

    protected virtual void OnBecameInvisible()
    {
        Deactivate();
    }

    protected virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
