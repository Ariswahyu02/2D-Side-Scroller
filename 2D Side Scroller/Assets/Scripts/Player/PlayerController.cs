using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Player Stats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private float knockbackDuration = 0.2f;

    private bool isDead = false;
    private float knockbackTimer = 0f;
    private Vector2 playerLastDirection = Vector2.right;

    [SerializeField] private GameObject playerSprite;
    private Animator animator;

    void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = playerSprite.GetComponent<Animator>();
        currentHealth = maxHealth;
        // Weapon assignment should be handled elsewhere (e.g., inventory, pickup, etc.)

        playerControls.Player.InventoryOpen.performed += ctx => GameUI.Instance.ToggleInventory();
    }

    void OnEnable()
    {
        playerControls.Enable();

        // playerControls.Weapon.Shoot.performed += ctx => Shoot();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        GameUI.Instance.UpdateHealthUI(currentHealth, maxHealth);
    }

    void Update()
    {
        if (isDead) return;
        moveInput = playerControls.Player.Move.ReadValue<Vector2>();

        if (playerControls.Weapon.Shoot.IsPressed()) Shoot();
        else InventoryManager.Instance.currentWeapon?.StopAnimation();

        FlipSprite(moveInput.x);
    }

    void FixedUpdate()
    {
        if (isDead) return;
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            return;
        }
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void Shoot()
    {
        if (IsDead()) return;

        Debug.Log(InventoryManager.Instance.currentWeapon);
        if (InventoryManager.Instance.currentWeapon != null)
        {
            InventoryManager.Instance.currentWeapon.directionShot = playerLastDirection;
            InventoryManager.Instance.currentWeapon.Attack();
        }
    }

    public void TakeDamage(int damage, Vector2 knockbackDir)
    {
        if (isDead) return;
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        GameUI.Instance.UpdateHealthUI(currentHealth, maxHealth);
        rb.linearVelocity = knockbackDir;
        knockbackTimer = knockbackDuration;
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        animator.SetTrigger("DamagedTrigger");
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        // Play death animation, disable controls, etc.
        Debug.Log("Player died!");
        InventoryManager.Instance.currentWeapon?.StopAnimation();
        // Optionally: gameObject.SetActive(false);
    }

    private void FlipSprite(float directionX)
    {
        if (directionX != 0)
        {
            Vector3 scale = transform.localScale;
            // scale.x = directionX > 0 ? 1 : -1;

            if (directionX > 0)
            {
                scale.x = 1;
                playerLastDirection = Vector2.right;
            }
            else
            {
                scale.x = -1;
                playerLastDirection = Vector2.left;
            }

            transform.localScale = scale;
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
