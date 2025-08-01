using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour, IReset
{
    [Header("-------Movement-------")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveDirection;

    [Header("-------Jump-------")]
    [SerializeField] private float jumpStrength;
    [SerializeField] private float jumpCutMultiplier;
    [SerializeField] private float jumpBufferDuration;
    private float jumpBufferTimer;

    [Header("-------GroundCheck-------")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    public bool isGrounded;

    [Header("-------Coyote Time-------")]
    [SerializeField] private float coyoteTimeDuration;
    private float coyoteTimeTimer;

    [Header("-------Objects-------")]
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public Vector2 initialPosition;
    public bool isActive;

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

        jumpBufferTimer -= Time.deltaTime;

        animator.SetFloat("xSpeed", rb.linearVelocityX);
        animator.SetFloat("ySpeed", rb.linearVelocityY);
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = moveDirection.x * moveSpeed;

        //Se o jumpBuffer estiver ativo
        if (jumpBufferTimer >= Mathf.Epsilon)
        {
            if
            (
                isGrounded ||
                rb.linearVelocityY < 0f && coyoteTimeTimer >= Mathf.Epsilon
            )
            {
                rb.linearVelocityY = jumpStrength;
                jumpBufferTimer = 0f;

                animator.SetTrigger("jump");
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
            jumpBufferTimer = jumpBufferDuration;

        else if (context.canceled && rb.linearVelocityY > 0f)
            rb.linearVelocityY *= jumpCutMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            //Chamar efeitos de morte

            playerInput.enabled = false;
            spriteRenderer.enabled = false;
        }
    }

    public void Reset()
    {
        //Volta o personagem à posição inicial  
        transform.position = initialPosition;

        //Reativa sprite renderer
        spriteRenderer.enabled = true;

        //SE o personagem atual estiver ativo, reativa o playerINput também
        if (isActive)
            playerInput.enabled = true;
    }
}