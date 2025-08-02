using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour, IReset
{
    [Header("-------Movement-------")]
    [SerializeField] private float moveSpeed;
    [HideInInspector] public Vector2 moveDirection;

    [Header("-------Jump-------")]
    [SerializeField] private float jumpStrength;
    [SerializeField] private float coyoteTimeDuration;
    private float coyoteTimeTimer;

    [Header("-------GroundCheck-------")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    public bool isGrounded;

    [Header("-------Events-------")]
    public UnityEvent characterDied;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [HideInInspector] public Vector2 initialPosition;
    public bool isActive;
    public bool jumpPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        coyoteTimeTimer = coyoteTimeDuration;

        initialPosition = transform.position;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        if (isGrounded)
            coyoteTimeTimer = coyoteTimeDuration;
        else
            coyoteTimeTimer -= Time.deltaTime;

        animator.SetFloat("xSpeed", rb.linearVelocityX);
        animator.SetFloat("ySpeed", rb.linearVelocityY);
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = moveDirection.x * moveSpeed;
    }

    private void LateUpdate()
    {
        jumpPressed = false;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
            ApplyJump();
    }

    public void ApplyJump()
    {
        if
        (
            isGrounded ||
            rb.linearVelocityY < 0f && coyoteTimeTimer >= Mathf.Epsilon
        )
        {
            jumpPressed = true;

            rb.linearVelocityY = jumpStrength;

            animator.SetTrigger("jump");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike") || other.CompareTag("Enemy"))
        {
            playerInput.enabled = false;
            spriteRenderer.enabled = false;

            rb.linearVelocity = Vector2.zero;

            characterDied.Invoke();
        }
    }

    public void Reset()
    {
        transform.position = initialPosition;

        spriteRenderer.enabled = true;

        if (isActive)
            playerInput.enabled = true;
    }
}