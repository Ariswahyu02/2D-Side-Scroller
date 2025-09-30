using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [SerializeField] private float moveSpeed = 5f;
    // Weapon system
    public Weapon currentWeapon;

    void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        // Weapon assignment should be handled elsewhere (e.g., inventory, pickup, etc.)
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

    }

    void Update()
    {
        moveInput = playerControls.Player.Move.ReadValue<Vector2>();

        if (playerControls.Weapon.Shoot.IsPressed()) Shoot();
        else currentWeapon?.StopAnimation();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        rb.MovePosition(rb.position + moveInput * Time.fixedDeltaTime * moveSpeed);
    }
    
    void Shoot()
    {
        Debug.Log(currentWeapon);
        if (currentWeapon != null)
        {
            currentWeapon.Attack();
        }
    }
}
