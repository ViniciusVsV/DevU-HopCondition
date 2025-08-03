using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour, IReset
{
    public bool collided;

    [Header("-------Movement-------")]
    [SerializeField] private float moveSpeed;
    public Vector2 moveDirection;

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
    private Collider2D col;

    private float baseGravityScale;

    private Vector2 initialPosition;
    public bool isActive;
    [HideInInspector] public bool jumpPressed;
    public bool isDead;
    private bool isFacingRight;
    public bool carrotReached;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        coyoteTimeTimer = coyoteTimeDuration;

        baseGravityScale = rb.gravityScale;

        initialPosition = transform.position;

        isFacingRight = true;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

        if (moveDirection.x > 0f && !isFacingRight || moveDirection.x < 0f && isFacingRight)
            Flip();

        if (isGrounded)
            coyoteTimeTimer = coyoteTimeDuration;
        else
            coyoteTimeTimer -= Time.deltaTime;

        animator.SetFloat("xSpeed", Mathf.Abs(rb.linearVelocityX));
        animator.SetFloat("ySpeed", rb.linearVelocityY);

        collided = false;
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocityX = 0f;
            return;
        }

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
        if (isDead)
            return;

        if
        (
            isGrounded ||
            rb.linearVelocityY < 0f && coyoteTimeTimer >= Mathf.Epsilon
        )
        {
            jumpPressed = true;

            rb.linearVelocityY = jumpStrength;
            SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.jumpSound, transform, SoundFXManager.Instance.lowVolume, SoundFXManager.Instance.lowPitch);
            animator.SetTrigger("jump");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collided)
        {
            return;
        }

        if (other.CompareTag("Spike") || other.CompareTag("Enemy") || other.CompareTag("Laser"))
        {
            collided = true;
            isDead = true;

            col.enabled = false;

            playerInput.enabled = false;
            spriteRenderer.enabled = false;

            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;

            characterDied.Invoke();
        }

        else if (other.CompareTag("Carrot"))
        {
            collided = true;

            carrotReached = true;
            isDead = true;
        }
    }

    public void _Reset(bool reactivate)
    {
        playerInput.enabled = false;

        isDead = false;
        carrotReached = false;

        col.enabled = true;

        transform.position = initialPosition;

        transform.localScale = new Vector2(1f, 1f);
        isFacingRight = true;

        spriteRenderer.enabled = true;

        rb.gravityScale = baseGravityScale;

        moveDirection = Vector2.zero;

        if (isActive)
            playerInput.enabled = true;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
    }
}