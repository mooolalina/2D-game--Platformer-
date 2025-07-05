using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Ссылки на UI-сердечки
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    private Vector2 moveInput;

    [SerializeField] private float speed = 2f; // скорость персонажа
    [SerializeField] private float jumpForce = 6f; // высота прыжка
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private int maxLives = 3; // кол-во жизней перса
    [SerializeField] private Transform attackPoint; // точка атаки (например, перед персонажем)
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers; // слой врагов
    [SerializeField] private int attackDamage = 1;
    private int currentLives;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentLives = maxLives;
    }

    private void OnEnable()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions["Move"].performed += OnMove;
            playerInput.actions["Move"].canceled += OnMove;
            playerInput.actions["Jump"].performed += OnJump;
            playerInput.actions["Attack"].performed += OnAttack;
        }
    }

    private void OnDisable()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions["Move"].performed -= OnMove;
            playerInput.actions["Move"].canceled -= OnMove;
            playerInput.actions["Jump"].performed -= OnJump;
            playerInput.actions["Attack"].performed -= OnAttack;
        }
    }

    private void Update()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isGrounded)
        {
            animator.SetInteger("state", 2);
        }
        else
        {
            if (!wasGrounded && isGrounded)
            {
                if (Mathf.Abs(moveInput.x) > 0.1f)
                    animator.SetInteger("state", 1); // run
                else
                    animator.SetInteger("state", 0); // idle
            }
            else
            {
                if (Mathf.Abs(moveInput.x) > 0.1f)
                    animator.SetInteger("state", 1); // run
                else
                    animator.SetInteger("state", 0); // idle
            }
        }

        // Поворот персонажа в сторону движения
        if (moveInput.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetInteger("state", 2);
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Attack();
    }

    private void Attack()
    {
        Debug.Log("Атака!");
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            MushroomEnemy enemyScript = enemy.GetComponent<MushroomEnemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        if (currentLives < 0) currentLives = 0;

        Debug.Log($"О нет, вы ранены! Жизней осталось: {currentLives}");

        UpdateHeartsUI();

        if (currentLives <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Вы погибли!");
        // Тут потом добавить код на перезапуск уровня сначала или чтобы в меню выбрасывало
        enabled = false;
    }
    
    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLives)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

}
