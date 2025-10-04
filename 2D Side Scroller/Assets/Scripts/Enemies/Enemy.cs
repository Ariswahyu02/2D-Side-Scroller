using UnityEngine;

public enum EnemyState
{
    Idle,
    Chase,
    Returning
}

public class Enemy : MonoBehaviour
{
    public EnemyState currentState = EnemyState.Idle;
    [SerializeField] private PlayerController player;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private int health = 50;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float damageCooldown = 0.5f;
    [SerializeField] private float returnDelay = 2f;
    [SerializeField] private Transform enemySprite;
    private Rigidbody2D rb;
    private Vector3 originPosition;
    private float lastDamageTime = -999f;
    private float returnTimer = 0f;

    [Header("Chase Range Collider")]
    [SerializeField] private CircleCollider2D chaseRangeCollider;

    [Header("Enemy UI")]
    [SerializeField] private EnemyUI enemyUI;

    [Header("On Enemy Died")]
    [SerializeField] private WeaponBuffType buffDropWhenDied;
    [SerializeField] private WeaponType weaponDropWhenDied;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originPosition = transform.position;
        if (chaseRangeCollider != null) chaseRangeCollider.isTrigger = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        health = maxHealth;
    }

    protected virtual void Update()
    {
        bool playerDead = player != null && player.IsDead();

        if (playerDead)
        {
            if (currentState != EnemyState.Returning)
            {
                currentState = EnemyState.Returning;
                returnTimer = returnDelay;
            }
        }
        else if (currentState == EnemyState.Returning && !playerDead)
        {
            currentState = EnemyState.Idle;
        }

        switch (currentState)
        {
            case EnemyState.Chase:
                Vector2 direction = (player.gameObject.transform.position - transform.position).normalized;
                rb.linearVelocity = direction * moveSpeed;
                FlipSprite(direction.x);
                break;
            case EnemyState.Idle:
                rb.linearVelocity = Vector2.zero;
                break;
            case EnemyState.Returning:
                rb.linearVelocity = Vector2.zero;
                if (returnTimer > 0f)
                {
                    returnTimer -= Time.deltaTime;
                }
                else
                {
                    ReturnToOrigin();
                }
                break;
        }
    }

    protected virtual void ReturnToOrigin()
    {
        if (Vector2.Distance(transform.position, originPosition) > 0.1f)
        {
            Vector2 direction = (originPosition - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
            FlipSprite(direction.x);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            currentState = EnemyState.Idle;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        enemyUI.UpdateEnemyHealthBar();
        if (health <= 0)
        {
            Die();
            return;
        }

        SoundManager.Instance.PlaySFX("Enemy Hit");
    }

    public int GetCurrentEnemyHealth()
    {
        return health;
    }

    public int GetMaxEnemyHealth()
    {
        return maxHealth;
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamagePlayer(collision);
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision);
    }

    private void TryDamagePlayer(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                lastDamageTime = Time.time;
                Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(10, knockbackDir * knockbackForce);
            }
        }
    }

    protected virtual void Die()
    {
        gameObject.SetActive(false);
        if (buffDropWhenDied == WeaponBuffType.FireRateUp && weaponDropWhenDied == WeaponType.None)
        {
            DroppedItemPoolManager.Instance.GetBuff(buffDropWhenDied, transform.position, 0f);
        }
        else if (buffDropWhenDied == WeaponBuffType.PowerUp && weaponDropWhenDied == WeaponType.None)
        {
            DroppedItemPoolManager.Instance.GetBuff(buffDropWhenDied, transform.position, 0f);
        }
        else if (weaponDropWhenDied != WeaponType.Glock && buffDropWhenDied == WeaponBuffType.None)
        {
            DroppedItemPoolManager.Instance.GetWeaponPickup(weaponDropWhenDied, transform.position, 0f);
        }

        SoundManager.Instance.PlaySFX("Enemy Died");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (player != null && !player.IsDead())
            {
                currentState = EnemyState.Chase;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !player.IsDead())
        {
            currentState = EnemyState.Returning;
            returnTimer = returnDelay;
        }
    }

    protected void FlipSprite(float directionX)
    {
        float flipValue = Mathf.Abs(enemySprite.transform.localScale.x);
        enemySprite.transform.localScale = new Vector3(directionX > 0 ? flipValue : -flipValue, enemySprite.transform.localScale.y, enemySprite.transform.localScale.z);
    }
}
